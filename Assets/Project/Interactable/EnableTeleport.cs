using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTeleport : MonoBehaviour
{
    public bool Activate = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Teleport.CanTeleport = Activate;
        }
    }
}
