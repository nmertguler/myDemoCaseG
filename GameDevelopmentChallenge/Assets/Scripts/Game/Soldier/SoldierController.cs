using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SoldierController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    public void MoveToTarget(Vector3 targetPos)
    {
        agent.SetDestination(targetPos);
    }
}
