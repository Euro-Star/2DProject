using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameUtils;
using UnityEngine.UI;

public class LoadingSceneManager : UIBase
{
    public static string nextScene;
    private Scrollbar progressBar;

    enum Scrollbars
    {
        Bar_Loading
    }

    private void Awake()
    {
        Bind<Scrollbar>(typeof(Scrollbars));

        progressBar = Get<Scrollbar>((int)Scrollbars.Bar_Loading);
    }

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.size = Mathf.Lerp(progressBar.size, op.progress, timer);
                if (progressBar.size >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                progressBar.size = Mathf.Lerp(progressBar.size, 1f, timer);
                if (progressBar.size == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}