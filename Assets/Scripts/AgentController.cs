using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AgentController : MonoBehaviour {
    private NavMeshAgent navMeshAgent;

    public void Setup() {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update() {

    }

    public void SetDestination(Vector3 dest) {
        navMeshAgent.SetDestination(dest);
    }
}
