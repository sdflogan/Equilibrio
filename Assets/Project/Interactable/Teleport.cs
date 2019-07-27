using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Walkable To;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("teleportando!");
            PlayerController.Instance.Teleport(To.GetDestination(To.transform.position));
        }
    }
}
