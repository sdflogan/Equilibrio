﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class SceneLoader : Singleton<SceneLoader>
{
    public Image LoadImage;
    public float TransitionTime = 1f;
    public bool DebugFlag = false;
    public int StartLoadScene = -1;
    public int current = -1;

    private void Start()
    {
        if (DebugFlag)
        {
            DualRenderCamera.Instance.SetReferences();
            LoadImage.DOFade(0f, TransitionTime).Play();
        }

        if (StartLoadScene != -1)
        {
            SceneManager.LoadScene(StartLoadScene, LoadSceneMode.Additive);
            LoadImage.DOFade(0f, TransitionTime).Play();
        }
    }

    public void LoadSceneFade(int scene)
    {
        Debug.Log("Cargando");
        LoadImage.DOFade(1f, TransitionTime).OnComplete(() => LoadScene(scene)).Play();
        FmodController.Instance.LoadScene(scene);
    }

    public void LoadScene(int scene)
    {
        current = scene;
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
        float toLoad = (current != -1 ? current : sceneIndex - 1);
            
        if (toLoad >= 0)
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
