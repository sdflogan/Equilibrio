using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Walkable : MonoBehaviour
{
    public List<Transform> TargetPositions;

    public Vector3 GetDestination(Vector3 position)
    {
        Transform nearest = null;
        float closestDistance = 9999;
        float tmpDist;

        foreach(Transform t in TargetPositions)
        {
            tmpDist = Vector3.Distance(t.position, position);

            if (nearest == null || tmpDist < closestDistance)
            {
                nearest = t;
                closestDistance = tmpDist;
            } 
        }

        return nearest.position;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (TargetPositions != null)
        {
            foreach (Transform t in TargetPositions)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(t.position, 0.1f);
            }
        }
    }
#endif
}
