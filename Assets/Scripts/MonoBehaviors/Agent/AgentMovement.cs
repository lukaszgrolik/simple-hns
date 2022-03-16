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
            navMeshAgent.speed = agentController.Agent.movement.CurrentSpeed;

            agentController.Agent.movement.speedChanged += OnSpeedChanged;
            agentController.Agent.movement.destinationChanged += OnDestinationChanged;
            agentController.Agent.movement.cancelled += OnCancelled;
            agentController.Agent.movement.disabled += OnDisabled;
        }

        // public void SetDestination(Vector3 dest)
        // {
        //     if (movementTargetObj) Destroy(movementTargetObj);

        //     movementTargetObj = InstantiateMovementTarget(dest);

        //     navMeshAgent.SetDestination(dest);
        // }

        void OnSpeedChanged(float speed)
        {
            navMeshAgent.speed = speed;
        }

        void OnDestinationChanged(Vector3 pos)
        {
            if (movementTargetObj) Object.Destroy(movementTargetObj);

            movementTargetObj = InstantiateMovementTarget(pos);

            navMeshAgent.SetDestination(pos);
        }

        void OnCancelled()
        {
            if (movementTargetObj) Object.Destroy(movementTargetObj);

            navMeshAgent.ResetPath();
        }

        void OnDisabled()
        {
            navMeshAgent.enabled = false;
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