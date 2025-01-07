using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEditor;

public class ServerManager : MonoBehaviour
{
    static private ServerManager serverManager;
    static public ServerManager inst { get { return serverManager; } }

    private void Awake()
    {
        serverManager = this;
    }

    public void UpdateAtk(int atk)
    {
        SignInManager.inst.firebaseData.GetReference("users").Child(SignInManager.inst.userKey).
            Child("atk").SetValueAsync(atk).ContinueWith(task =>
        {
            ServerLog("UpdateAtk", task.IsCompleted);
        });
    }

    public void UpdateExp(int exp)
    {
        SignInManager.inst.firebaseData.GetReference("users").Child(SignInManager.inst.userKey).
            Child("exp").SetValueAsync(exp).ContinueWith(task =>
        {
            ServerLog("UpdateExp", task.IsCompleted);
        });
    }

    public void UpdateHp(int hp)
    {
        SignInManager.inst.firebaseData.GetReference("users").Child(SignInManager.inst.userKey).
            Child("hp").SetValueAsync(hp).ContinueWith(task =>
        {
            ServerLog("UpdateHp", task.IsCompleted);
        });
    }

    public void UpdateLevel(int level)
    {
        SignInManager.inst.firebaseData.GetReference("users").Child(SignInManager.inst.userKey).
            Child("level").SetValueAsync(level).ContinueWith(task =>
        {
            ServerLog("UpdateLevel", task.IsCompleted);
        });
    }

    public void UpdateMoney(int moeny)
    {
        SignInManager.inst.firebaseData.GetReference("users").Child(SignInManager.inst.userKey).
            Child("money").SetValueAsync(moeny).ContinueWith(task =>
        {
            ServerLog("UpdateMoney", task.IsCompleted);
        });
    }

    public void UpdateStatPoint(int statPoint)
    {
        SignInManager.inst.firebaseData.GetReference("users").Child(SignInManager.inst.userKey).
            Child("statPoint").SetValueAsync(statPoint).ContinueWith(task =>
        {
            ServerLog("UpdateStatPoint", task.IsCompleted);
        });
    }

    public void UpdateSkillLevel(int skillCode, int level)
    {
        SignInManager.inst.firebaseData.GetReference("users").Child(SignInManager.inst.userKey).
            Child("skillLevel").Child(skillCode.ToString()).SetValueAsync(level).ContinueWith(task =>
        {
            ServerLog("UpdateSkillLevel", task.IsCompleted);
        });
    }

    private void ServerLog(string msg, bool isComplete) 
    {
        if (isComplete)
        {
            Debug.Log("Server : " + msg + "is Complete");
        }
        else
        {
            Debug.LogError("Server : " + msg + "is Failed");
        }
    }
}
