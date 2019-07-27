using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public void LoadScene(int scene)
    {
        StartCoroutine(LoadSceneBackground(scene));
    }

    private IEnumerator LoadSceneBackground(int sceneIndex)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

        while (!load.isDone)
        {
            yield return null;
        }

        load = SceneManager.UnloadSceneAsync(sceneIndex-1);

        while (!load.isDone)
        {
            yield return null;
        }
    }
}
