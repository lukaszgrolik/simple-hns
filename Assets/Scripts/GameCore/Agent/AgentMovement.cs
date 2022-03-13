using UnityEngine;

namespace GameCore
{
    public class AgentMovement
    {
        private AgentHealth agentHealth;

        private bool isChangingDestination = false;
        public bool IsChangingDestination => isChangingDestination;

        private Vector2 currentDestination;
        public Vector2 CurrentDestination => currentDestination;

        public event System.Action<Vector2> destinationChanged;
        public event System.Action cancelled;
        public event System.Action arrived;
        public event System.Action stopped;
        public event System.Action disabled;

        public AgentMovement(AgentHealth agentHealth)
        {
            this.agentHealth = agentHealth;

            agentHealth.died += OnAgentDied;
        }

        public void SetDestination(Vector2 pos)
        {
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
    }
}