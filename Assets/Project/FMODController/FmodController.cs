using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FmodController : Singleton<FmodController>
{
    public string musicEvt = "event:/Music";           // string reference to the FMOD-authored Event named "Loop"; name will appear in the Unity Inspector
    FMOD.Studio.EventInstance Music;             // Unity EventInstance name for Loop event that was created in FMOD
    FMOD.Studio.ParameterInstance parameter;

    private void Awake()
    {
        Music = FMODUnity.RuntimeManager.CreateInstance(musicEvt);
        Music.getParameter("Level_Select", out parameter);
        //parameter.setValue(0.33f);
        Music.start();
    }
}
