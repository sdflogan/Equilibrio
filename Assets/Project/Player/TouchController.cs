using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    public LayerMask InteractableMask;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100f, InteractableMask))
            {
                Walkable target = hit.transform.GetComponent<Walkable>();

                if (target != null)
                {
                    PlayerController.Instance.Move(target.GetDestination());
                }
            }
        }
    }
}
