using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEvent : SliderEvent
{
    public Transform StartPosition;
    public Transform CorrectPosition;
    public Transform EndPosition;

    protected override void DoActionInsideRange(float value)
    {
        // Enable Floor
    }

    protected override void DoActionOutsideRange(float value)
    {
         if (value < ActionRange.x)
        {
            // startPosition -> CorrectPosition
        }
        else
        {
            // CorrectPosition -> EndPosition
        }
    }
}
