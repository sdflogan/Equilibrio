using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHelper : MonoBehaviour
{
    public SliderAnimation SliderAnim;

    public void AnimationEnd()
    {
        SliderAnim.CheckAnimationStatus(true);
    }

    public void AnimationStart()
    {
        SliderAnim.CheckAnimationStatus(false);
    }
}
