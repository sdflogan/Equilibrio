using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public int nextLevel;
    public bool endGame = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("CargandoNivel");
            SceneLoader.Instance.LoadSceneFade(nextLevel);

            if (endGame)
            {
                FmodController.Instance.EndGame(1);
            }
            else
            {
                FmodController.Instance.NextLevel();
            }
        }
    }
}
