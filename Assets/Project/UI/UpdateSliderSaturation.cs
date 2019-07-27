using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSliderSaturation : MonoBehaviour
{
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Start()
    {
        slider.value = DualRenderCamera.Instance.GetMainSaturation();
    }

    private void Update()
    {
        slider.value = DualRenderCamera.Instance.GetMainSaturation();
    }
}
