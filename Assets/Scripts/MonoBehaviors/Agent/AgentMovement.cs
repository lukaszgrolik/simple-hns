using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviors
{
    public class AgentMovement
    {
        private AgentController agentController;
        private NavMeshAgent navMeshAgent;

        // public class ArrivedEvent : UnityEvent { }
        // public readonly ArrivedEvent arrived = new ArrivedEvent();

        private GameObject movementTargetObj;

        public AgentMovement(
            AgentController agentController,
            NavMeshAgent navMeshAgent
        )
        {
            this.agentController = agentController;
            this.navMeshAgent = navMeshAgent;

            navMeshAgent.updateRotation = false;

            agentController.Agent.movement.destinationChanged += OnDestinationChanged;
            agentController.Agent.movement.cancelled += OnCancelled;
        }

        // public void SetDestination(Vector3 dest)
        // {
        //     if (movementTargetObj) Destroy(movementTargetObj);

        //     movementTargetObj = InstantiateMovementTarget(dest);

        //     navMeshAgent.SetDestination(dest);
        // }

        void OnDestinationChanged(Vector2 pos)
        {
            var dest = new Vector3(pos.x, 0, pos.y);
            if (movementTargetObj) Object.Destroy(movementTargetObj);

            movementTargetObj = InstantiateMovementTarget(dest);

            navMeshAgent.SetDestination(dest);
        }

        void OnCancelled()
        {
            if (movementTargetObj) Object.Destroy(movementTargetObj);

            navMeshAgent.ResetPath();
        }

        GameObject InstantiateMovementTarget(Vector3 dest)
        {
            var gm = agentController.GameplayManager;
            var obj = Object.Instantiate(gm.MovementTargetPrefab, dest, Quaternion.identity, gm.MovementTargetContainer);
            var movementTarget = obj.GetComponent<MovementTarget>();
            movementTarget.Setup(agentController);

            return obj;
        }

        // void OnAgentDied()
        // {
        //     throw new System.NotImplementedException();
        // }
    }
}