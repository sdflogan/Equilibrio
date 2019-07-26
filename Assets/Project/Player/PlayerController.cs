using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : Singleton<PlayerController>
{
    private NavMeshAgent m_Agent;

    private void Awake()
    {
        m_Agent = GetComponentInChildren<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector3 dest)
    {
        m_Agent.SetDestination(dest);
    }
}
