using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public int nextLevel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SceneLoader.Instance.LoadScene(nextLevel);
        }
    }
}
