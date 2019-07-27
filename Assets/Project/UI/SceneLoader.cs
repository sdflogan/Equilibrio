using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneLoader : Singleton<SceneLoader>
{
    public Image LoadImage;
    public float TransitionTime = 1f;

    private void Start()
    {
        //LoadImage.DOFade(1f, TransitionTime).OnComplete(() => LoadScene(0)).Play();
        LoadScene(0);
    }

    public void LoadSceneFade(int scene)
    {
        LoadImage.DOFade(1f, TransitionTime).OnComplete(() => LoadScene(scene)).Play();
    }

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

        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneIndex));

        DualRenderCamera.Instance.SetReferences();

        if (sceneIndex - 1 >= 0)
        {
            load = SceneManager.UnloadSceneAsync(sceneIndex - 1);

            while (!load.isDone)
            {
                yield return null;
            }
        }

        LoadImage.DOFade(0f, TransitionTime).Play();
    }
}
