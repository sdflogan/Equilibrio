using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkableManager : Singleton<WalkableManager>
{
    public List<Walkable> UnlockedPoints;
    public List<Walkable> LockedPoints;

    public void Unlock(Walkable point)
    {
        if (!UnlockedPoints.Contains(point))
        {
            UnlockedPoints.Add(point);
            LockedPoints.Remove(point);
        }
    }

    public void Lock(Walkable point)
    {
        if (!LockedPoints.Contains(point))
        {
            UnlockedPoints.Remove(point);
            LockedPoints.Add(point);
        }

        CheckAgent();
    }

    public void UnlockAll()
    {
        foreach(Walkable w in LockedPoints)
        {
            UnlockedPoints.Add(w);
        }
        LockedPoints.Clear();
    }

    public void Unlock(List<Walkable> points)
    {
        foreach (Walkable w in points)
        {
            if (!UnlockedPoints.Contains(w))
            {
                UnlockedPoints.Add(w);
                LockedPoints.Remove(w);
            }
        }
    }

    public void Lock(List<Walkable> points)
    {
        foreach (Walkable w in points)
        {
            if (!LockedPoints.Contains(w))
            {
                LockedPoints.Add(w);
                UnlockedPoints.Remove(w);
            }
        }

        CheckAgent();
    }

    private void CheckAgent()
    {
        if (LockedPoints.Contains(PlayerController.Instance.GetTarget()))
        {
            PlayerController.Instance.Move(transform.position);
        }
    }

    public bool IsUnlocked(Walkable target)
    {
        return UnlockedPoints.Contains(target);
    }
}
