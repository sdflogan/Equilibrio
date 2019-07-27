using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SliderAlpha : SliderEvent
{
    public MeshRenderer TargetMeshRenderer;
    public GameObject Obstacle;
    public float FadeTime = 0.25f;
    private bool m_Unlocked = false;
    private Material[] m_Materials;

    private void Awake()
    {
        m_Materials = TargetMeshRenderer.sharedMaterials;
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
            Fade(1);
        }
    }

    public void LockPath()
    {
        if (m_Unlocked)
        {
            m_Unlocked = false;
            Obstacle.SetActive(true);
            Fade(0);
        }
    }

    public void Fade(float value)
    {
        for (int i=0; i<m_Materials.Length; i++)
        {
            m_Materials[i].DOFade(value, FadeTime).Play();
        }
    }
}
