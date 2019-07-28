using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderObstacle : SliderEvent
{
    public GameObject Obstacle;
    public float FadeTime = 0.25f;
    private bool m_Unlocked = false;

    private void Awake()
    {

    }

    protected override void DoActionInsideRange(float value)
    {
        UnlockPath();
    }

    protected override void DoActionOutsideRange(float value)
    {
        LockPath();
    }

    public void UnlockPath()
    {
        if (!m_Unlocked)
        {
            m_Unlocked = true;
            Obstacle.SetActive(false);
        }
    }

    public void LockPath()
    {
        if (m_Unlocked)
        {
            m_Unlocked = false;
            Obstacle.SetActive(true);
        }
    }
}
