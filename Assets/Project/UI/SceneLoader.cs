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
    public bool Debug = false;
    public int StartLoadScene = -1;

    private void Start()
    {
        if (Debug)
        {
            DualRenderCamera.Instance.SetReferences();
            LoadImage.DOFade(0f, TransitionTime).Play();
        }

        if (StartLoadScene != -1)
        {
            SceneManager.LoadScene(StartLoadScene, LoadSceneMode.Additive);
        }
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
