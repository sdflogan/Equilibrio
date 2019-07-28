using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    public LayerMask InteractableMask;
    public GameObject canvasClick;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f, InteractableMask))
            {
                Walkable target = hit.transform.GetComponent<Walkable>();

                canvasClick.transform.position = hit.transform.position + new Vector3(-1.5f, 1.46f, 1.5f);
                canvasClick.GetComponent<Animator>().Play(0);

                if (target != null)
                {
                    PlayerController.Instance.Move(target, hit.point);
                }
            }
        }
    }
}
