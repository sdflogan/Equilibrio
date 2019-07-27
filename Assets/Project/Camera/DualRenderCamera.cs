using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DualRenderCamera : Singleton<DualRenderCamera>
{
    public Camera MainCamera;
    public Camera SecondCamera;

    PostProcessVolume mainVolume;
    PostProcessVolume secondVolume;

    ColorGrading mainGrading;

    public void SetReferences()
    {
        MainCamera = Camera.main;
        SecondCamera = GameObject.FindGameObjectWithTag("SecondCamera").GetComponent<Camera>();

        SecondCamera.gameObject.SetActive(false);
        SecondCamera.gameObject.SetActive(true);
        mainVolume = MainCamera.GetComponent<PostProcessVolume>();
        secondVolume = SecondCamera.GetComponent<PostProcessVolume>();

        mainVolume.profile.TryGetSettings(out mainGrading);
    }

    public void SetMainSaturation(float saturation)
    {
        mainGrading.saturation.value = saturation;
    }

    public void ModifyMainSaturation(float modify, float minValue, float maxValue)
    {
        mainGrading.saturation.value = Mathf.Clamp(mainGrading.saturation.value + modify, minValue, maxValue);
    }

    public float GetMainSaturation()
    {
        float value = 0;

        if (mainGrading != null)
        {
            value = mainGrading.saturation.value;
        }

        return value;
    }
}
