using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Google;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SignInManager : MonoBehaviour
{
    //public TextMeshPro infoText;
    protected string webClientId = "975010787033-1jb0nhf37tnmf46hopgumdkjuvcnakol.apps.googleusercontent.com";

    private FirebaseAuth auth;
    private GoogleSignInConfiguration configuration;
    private FirebaseDatabase database;
    private string userId;
    private string email;

    private static SignInManager signInManager;
    public static SignInManager inst { get { return signInManager; } }
    public FirebaseDatabase firebaseData { get { return database; } }
    public string userKey { get { return userId; } }

    private void Awake()
    {
        signInManager = this;
    }

    private void Start()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        database = FirebaseDatabase.DefaultInstance;

        CheckFirebaseDependencies();
    }  

    // 유저 데이터 삽입 및 수정 //
    public void CreateUser()
    {
        PlayerData data = new PlayerData();

        string json = JsonUtility.ToJson(data);

        string key = userId;
        this.database.GetReference("users").Child(key).SetRawJsonValueAsync(json);
    }

    public void SignInWithGoogle() { OnSignIn(); }
    public void SignOutFromGoogle() { OnSignOut(); }


    //////////////////////////////
    // 로그인 기능 및 필요 기능 //

    private void CheckFirebaseDependencies()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                {
                    auth = FirebaseAuth.DefaultInstance;
                }
                else
                    Debug.Log("Could not resolve all Firebase dependencies: " + task.Result.ToString());
            }
            else
            {
                Debug.Log("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }

    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        Debug.Log("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void OnSignOut()
    {
        Debug.Log("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
        Debug.Log("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    Debug.Log("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    Debug.Log("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            Debug.Log("Canceled");
        }
        else
        {
            Debug.Log("Welcome: " + task.Result.DisplayName + "!");

            email = task.Result.Email;

            SignInWithGoogleOnFirebase(task.Result.IdToken);
        }
    }

    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                    Debug.Log("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                Debug.Log("Sign In Successful.");

                userId = task.Result.UserId;

                LoadUser(userId);
            }
        });
    }
    /////////////////////////////////////////////////////////////////////////////


    private void LoadUser(string userId)
    {
        PlayerData userData = new PlayerData();

        database.RootReference.Child("users").Child(userId).GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Fail Load");
            }
            else if (task.IsCompleted)
            {
                if (task.Result.Exists)
                {
                    DataSnapshot snapshot = task.Result;

                    userData = JsonUtility.FromJson<PlayerData>(snapshot.GetRawJsonValue());

                    Player.player.SetPlayerData(userData);
                }              
                else
                {
                    CreateUser();
                    Player.player.SetPlayerData(userData);
                }

                UIManager.inst.UIController(GameUI.SignInUI, false);
            }
        });
    }
}

