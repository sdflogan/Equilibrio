using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderEvent : MonoBehaviour
{
    public Vector2 ActionRange;

    void Start()  
    {
        SliderEventController.Instance.Subscribe(this);
    }

    void OnDestroy()
    {
        if (SliderEventController.Instance != null)
        {
            SliderEventController.Instance.Unsubscribe(this);
        }
    }

    private bool CheckRange(float value)
    {
        return !(value < ActionRange.x || value > ActionRange.y);
    }

    protected virtual void DoAction(float value)
    {

    }

    protected virtual void DoReverseAction(float value)
    {

    }

    public void TryDoAction(float value)
    {
        if (CheckRange(value))
        {
            DoAction(value);
        }
        else
        {
            DoReverseAction(value);
        }
    }
}
