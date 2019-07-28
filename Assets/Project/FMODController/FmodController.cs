using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FmodController : Singleton<FmodController>
{
    public string musicEvt = "event:/Music";           // string reference to the FMOD-authored Event named "Loop"; name will appear in the Unity Inspector
    FMOD.Studio.EventInstance Music;             // Unity EventInstance name for Loop event that was created in FMOD
    FMOD.Studio.ParameterInstance parameter;
    private bool endGame = false;

    private void Awake()
    {
        Music = FMODUnity.RuntimeManager.CreateInstance(musicEvt);
        Music.getParameter("Level_Select", out parameter);
        //parameter.setValue(0.33f);
        Music.start();
    }

    public void SpacePressed(float value)
    {
        value *= 100f;
        Music.getParameter("bar", out parameter);
        parameter.setValue(value);
    }

    public void PuzzleValue(float value)
    {
        Music.getParameter("ok", out parameter);
        parameter.setValue(value);
    }

    public void Success(float value)
    {
        Music.getParameter("completed", out parameter);
        parameter.setValue(value);
        StartCoroutine(ResetValues(1f, "completed", 0));
    }

    public void EndGame(float value)
    {
        Success(0);
        Music.getParameter("menu", out parameter);
        parameter.setValue(1);
        endGame = true;
    }

    public void NextLevel()
    {
        Success(0);
        Music.getParameter("Next_Level", out parameter);
        parameter.setValue(1);
        StartCoroutine(ResetValues(2f, "Next_Level", 0f));
    }

    public void LoadScene(int scene)
    {
        if (endGame && scene != 1)
        {
            EndGame(0);
        }

        float value = 0f;
        if (scene == 2)
        {
            value = 0.33f;
        }
        else if (scene == 3)
        {
            value = 0.66f;
        }
        else if (scene == 4)
        {
            value = 1f;
        }

        Music.getParameter("Level_Select", out parameter);
        parameter.setValue(value);
    }

    private IEnumerator ResetValues(float seconds, string parameterName, float reset)
    {
        yield return new WaitForSeconds(seconds);

        FMOD.Studio.ParameterInstance p;
        Music.getParameter(parameterName, out p);
        p.setValue(reset);
    }
}
