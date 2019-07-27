using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSliderSaturation : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Slider>().value = DualRenderCamera.Instance.GetMainSaturation();
    }
}
