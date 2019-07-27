using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SliderEventController.Instance.Enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SliderEventController.Instance.Enabled = true;
    }
}
