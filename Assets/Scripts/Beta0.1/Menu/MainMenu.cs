using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
        //LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    /*
    public Slider progressBar;
    public void LoadSceneAsync(int sceneIndex)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneIndex));
    }

    private IEnumerator LoadSceneAsyncCoroutine(int sceneIndex)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            progressBar.value = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            if (asyncOperation.progress >= 0.9f && progressBar.value >= 1f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }
    */
}
