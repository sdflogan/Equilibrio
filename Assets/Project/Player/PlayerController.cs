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

    public void Teleport(Vector3 position)
    {
        m_Agent.enabled = false;
        transform.position = position;
        Move(position);
        StartCoroutine(EnableAfterTeleport(0.1f));
    }

    private IEnumerator EnableAfterTeleport(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        m_Agent.enabled = true;
    }
}
