using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// attach to UI Text component (with the full text already there)

public class UITextTypeWriter : MonoBehaviour
{

    Text txt;
    string story;

    public float delayToStart;

    private float time;
    private bool flag;
    void Awake()
    {
        txt = GetComponent<Text>();
        story = txt.text;
        txt.text = "";
        flag = true;
        // TODO: add optional delay when to start

    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= delayToStart && flag)
        {
            flag = false;
            StartCoroutine("PlayText");
        }
    }

    IEnumerator PlayText()
    {
        bool color = false;
        foreach (char c in story)
        {
            if (c == '<')
            {
                color = true;
                continue;
            }
            else if(c == '>')
            {
                color = false;
                continue;
            }
            if(color)
            {
                txt.text += "<color=#F4CC70>" + c + "</color>"; 
            }
            else
            {
                txt.text += c;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

}