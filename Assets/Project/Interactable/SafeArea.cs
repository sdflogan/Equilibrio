using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("hello there");
        if (other.tag == "Player")
        {
            SliderEventController.Instance.Enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("hello ");
        SliderEventController.Instance.Enabled = true;
    }
}
