using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaturationEvent : SliderEvent
{
    public float modifier = 1f;

    public float minValue = -100f;
    public float maxValue = 100f;

    protected override void DoActionInsideRange(float value)
    {
        base.DoActionInsideRange(value);
        float normalizedSaturation = (200 * value) - 100;
        Debug.Log(value);
        DualRenderCamera.Instance.SetMainSaturation(normalizedSaturation);
        //Debug.Log("Cambiando saturación");
    }

    protected override void DoActionOutsideRange(float value)
    {
        base.DoActionOutsideRange(value);
        float normalizedSaturation = (200 * value) - 100;
        DualRenderCamera.Instance.SetMainSaturation(normalizedSaturation);
        //Debug.Log("Cambiando saturación");
    }

    public override void TryDoAction(float value)
    {
        if (base.CheckRange(value))
        {
            DoActionInsideRange(value);
        }
        else
        {
            DoActionOutsideRange(value);
        }
    }
}
