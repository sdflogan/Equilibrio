using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : Singleton<PlayerController>
{
    private NavMeshAgent m_Agent;
    private Walkable m_WalkableTarget;
    private Animator m_Anim;

    private void Awake()
    {
        m_Agent = GetComponentInChildren<NavMeshAgent>();
        m_Anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        m_Anim.SetFloat("Speed", m_Agent.velocity.magnitude);
    }

    public void Move(Walkable dest, Vector3 hitPoint)
    {
        m_Agent.SetDestination(dest.GetDestination(hitPoint));
        m_WalkableTarget = dest;
    }

    public void Move(Vector3 pos)
    {
        if (m_Agent.enabled)
            m_Agent.SetDestination(pos);
    }

    public Vector3 GetDestination()
    {
        return m_Agent.destination;
    }

    public Walkable GetTarget()
    {
        return m_WalkableTarget;
    }

    public void Teleport(Vector3 position, Vector3 walkAfter)
    {
        m_Agent.enabled = false;
        transform.position = position;
        Move(position);
        StartCoroutine(EnableAfterTeleport(0.1f, walkAfter));
    }

    private IEnumerator EnableAfterTeleport(float seconds, Vector3 walkAfter)
    {
        yield return new WaitForSeconds(seconds);
        m_Agent.enabled = true;
        Move(walkAfter);
    }
}
