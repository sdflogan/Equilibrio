using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaturationEvent : SliderEvent
{
    public float modifier = 1f;

    public float minValue = -100f;
    public float maxValue = 100f;

    protected override void DoAction(float value)
    {
        base.DoAction(value);
        float normalizedSaturation = (200 * value) - 100;
        DualRenderCamera.Instance.SetMainSaturation(normalizedSaturation);
    }

    protected override void DoReverseAction(float value)
    {
        base.DoReverseAction(value);
        float normalizedSaturation = (200 * value) - 100;
        DualRenderCamera.Instance.SetMainSaturation(normalizedSaturation);
    }

    /*
     * OldRange = (OldMax - OldMin)
if (OldRange == 0)
    NewValue = NewMin
else
{
    NewRange = (NewMax - NewMin)  
    NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin
}
     * */
}
