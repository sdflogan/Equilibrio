using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Walkable : MonoBehaviour
{
    public Transform TargetPosition;

    public Vector3 GetDestination()
    {
        return TargetPosition.position;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (TargetPosition != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(TargetPosition.position, 0.1f);
        }
    }
#endif
}
