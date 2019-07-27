using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderEventController : Singleton<SliderEventController>
{
    public float Value = 0;
    public float Modifier = 0.05f;
    public bool Increment = false;
    public bool Enabled = true;
    public Vector2 range;
    public float UpdateCycleSeconds = 0.25f;

    private List<SliderEvent> m_Events = new List<SliderEvent>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateSlider(UpdateCycleSeconds));
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Increment = true;
        }
        else 
        {
            Increment = false;
        }
    }

    public void Subscribe(SliderEvent sliderEvent)
    {
        m_Events.Add(sliderEvent);
    }

    public void Unsubscribe(SliderEvent sliderEvent)
    {
        m_Events.Remove(sliderEvent);
    }

    private void Slider()
    {
        float tmp = (Increment ? Modifier : -Modifier);

        Value = Mathf.Clamp(Value + tmp, range.x, range.y);

        foreach (SliderEvent evt in m_Events)
        {
            evt.TryDoAction(Value);
        }
    }

    IEnumerator UpdateSlider(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (Enabled)
        {
            Slider();
        }

        StartCoroutine(UpdateSlider(seconds));
    }
}
