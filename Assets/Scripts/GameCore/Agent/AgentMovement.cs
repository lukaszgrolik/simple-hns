using UnityEngine;

namespace GameCore
{
    public class AgentMovement
    {
        public enum MovementMode
        {
            Walking,
            Running
        }

        private AgentHealth agentHealth;
        private AgentStun agentStun;

        private float walkingSpeed;
        private float runningSpeed;

        private MovementMode currentMovementMode;
        public MovementMode CurrentMovementMode => currentMovementMode;

        private float currentSpeed;
        public float CurrentSpeed => currentSpeed;

        private bool isChangingDestination = false;
        public bool IsChangingDestination => isChangingDestination;

        private Vector3 currentDestination;
        public Vector3 CurrentDestination => currentDestination;

        public event System.Action<float> speedChanged;
        public event System.Action<Vector3> destinationChanged;
        public event System.Action cancelled;
        public event System.Action arrived;
        public event System.Action stopped;
        public event System.Action disabled;

        public AgentMovement(
            AgentHealth agentHealth,
            AgentStun agentStun,
            float walkingSpeed,
            float runningSpeed
        )
        {
            this.agentHealth = agentHealth;
            this.agentStun = agentStun;

            this.walkingSpeed = walkingSpeed;
            this.runningSpeed = runningSpeed;

            agentHealth.died += OnAgentDied;
            agentStun.stunned += OnAgentStunned;

            currentMovementMode = MovementMode.Walking;
            currentSpeed = walkingSpeed;
        }

        public void SetWalkingMode()
        {
            currentMovementMode = MovementMode.Walking;
            currentSpeed = walkingSpeed;

            speedChanged.Invoke(currentSpeed);
        }

        public void SetRunningMode()
        {
            currentMovementMode = MovementMode.Running;
            currentSpeed = runningSpeed;

            speedChanged.Invoke(currentSpeed);
        }

        public void SetDestination(Vector3 pos)
        {
            if (agentStun.IsStunned) return;

            currentDestination = pos;
            isChangingDestination = true;

            destinationChanged.Invoke(pos);
        }

        public void Cancel()
        {
            if (!isChangingDestination) return;

            isChangingDestination = false;

            cancelled?.Invoke();
            stopped?.Invoke();
        }

        public void MarkArrived()
        {
            isChangingDestination = false;

            arrived?.Invoke();
            stopped?.Invoke();
        }

        void OnAgentDied(Agent agent)
        {
            Cancel();

            disabled?.Invoke();
        }

        void OnAgentStunned()
        {
            Cancel();
        }
    }
}