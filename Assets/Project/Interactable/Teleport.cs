using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Walkable To;
    public Transform WalkAfterTeleport;

    public static bool CanTeleport = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && CanTeleport)
        {
            CanTeleport = false;
            PlayerController.Instance.Teleport(To.GetDestination(To.transform.position), 
                WalkAfterTeleport.position);
        }
    }
}
