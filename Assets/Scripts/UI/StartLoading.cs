using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartLoading : MonoBehaviour
{
    private void Start()
    {
        LoadingSceneManager.LoadScene("Lobby");
    }
}