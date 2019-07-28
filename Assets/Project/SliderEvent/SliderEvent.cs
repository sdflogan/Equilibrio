using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderEvent : MonoBehaviour
{
    public Vector2 ActionRange;

    protected virtual void Start()  
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

    protected bool CheckRange(float value)
    {
        return !(value < ActionRange.x || value > ActionRange.y);
    }

    protected virtual void DoActionInsideRange(float value)
    {

    }

    protected virtual void DoActionOutsideRange(float value)
    {

    }

    public virtual void TryDoAction(float value)
    {
        if (CheckRange(value))
        {
            DoActionInsideRange(value);
            FmodController.Instance.PuzzleValue(1);
        }
        else
        {
            DoActionOutsideRange(value);
            FmodController.Instance.PuzzleValue(0);
        }
    }
}
