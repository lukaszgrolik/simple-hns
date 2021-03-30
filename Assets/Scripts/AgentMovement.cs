using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour {
    private AgentController agentController;
    private NavMeshAgent navMeshAgent;

    public class ArrivedEvent : UnityEvent {}
    private ArrivedEvent arrived = new ArrivedEvent();
    public ArrivedEvent Arrived => arrived;

    private GameObject movementTargetObj;

    public void Setup(AgentController agentController) {
        this.agentController = agentController;

        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
    }

    public void SetDestination(Vector3 dest) {
        if (movementTargetObj) Destroy(movementTargetObj);

        movementTargetObj = InstantiateMovementTarget(dest);

        navMeshAgent.SetDestination(dest);
    }

    public void Stop() {
        if (movementTargetObj) Destroy(movementTargetObj);

        navMeshAgent.ResetPath();
    }

    GameObject InstantiateMovementTarget(Vector3 dest) {
        var gm = agentController.GameplayManager;
        var obj = Instantiate(gm.MovementTargetPrefab, dest, Quaternion.identity, gm.MovementTargetContainer);
        var movementTarget = obj.GetComponent<MovementTarget>();
        movementTarget.Setup(agentController);

        return obj;
    }

    void OnAgentDied() {
        throw new System.NotImplementedException();
    }
}
