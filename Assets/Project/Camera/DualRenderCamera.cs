using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualRenderCamera : MonoBehaviour
{
    public Camera main;
    public Camera secondary;

    private void Awake()
    {
        secondary.gameObject.SetActive(true);
    }
}
