using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour {
    private NavMeshAgent navMeshAgent;

    public void Setup() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
    }

    public void SetDestination(Vector3 dest) {
        navMeshAgent.SetDestination(dest);
    }

    public void Stop() {
        navMeshAgent.ResetPath();
    }
}
