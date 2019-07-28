using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderAnimation : SliderEvent
{
    public Animator Animator;
    public GameObject Obstacle;
    public float Speed = 0;
    public float FadeTime = 0.25f;
    private bool m_Unlocked = false;
    private bool m_FirstTime = true;
    private bool m_Activated = false;

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
            Activate(true);
        }
    }

    public void LockPath()
    {
        if (m_Unlocked)
        {
            m_Unlocked = false;
            Obstacle.SetActive(true);
            Activate(false);
        }
    }

    public void Activate(bool value)
    {
        if (value)
        {
            Speed = 1;
            Animator.SetFloat("Speed", Speed);
            m_FirstTime = false;
            m_Activated = true;
        }
        else if (!m_FirstTime)
        {
            Speed = -1;
            Animator.SetFloat("Speed", Speed);
            m_Activated = false;
        }
    }

    public void CheckAnimationStatus(bool isEnd)
    {
        if (isEnd && m_Activated)
        {
            Speed = 0;
            Animator.SetFloat("Speed", Speed);
        }
        else if (!isEnd && !m_Activated)
        {
            Speed = 0;
            Animator.SetFloat("Speed", Speed);
        }
    }
}
