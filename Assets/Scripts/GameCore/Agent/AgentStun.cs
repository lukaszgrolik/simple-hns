using UnityEngine;

namespace GameCore
{
    public class AgentStun : AgentComponent
    {
        private AgentHealth agentHealth;

        private bool canBeStunned;
        private float stunTime;

        private bool isStunned = false;
        public bool IsStunned => isStunned;

        public event System.Action stunned;
        public event System.Action stunEnded;

        private float elapsedStunTime = 0f;

        public AgentStun(
            AgentHealth agentHealth,
            bool canBeStunned = false,
            float stunTime = 0f
        )
        {
            this.canBeStunned = canBeStunned;
            this.stunTime = stunTime;

            agentHealth.healthDecreased += OnAgentHealthDecreased;
        }

        public void OnUpdate(float deltaTime)
        {
            if (agent.health.isAlive && canBeStunned && isStunned)
            {
                elapsedStunTime += deltaTime;
                // Debug.Log($"elapsedStunTime: {elapsedStunTime} ({stunTime})");

                if (elapsedStunTime >= stunTime)
                {
                    elapsedStunTime = 0;

                    EndStun();
                }
            }
        }

        void Stun()
        {
            isStunned = true;

            stunned?.Invoke();
        }

        void EndStun()
        {
            isStunned = false;

            stunEnded?.Invoke();
        }

        void OnAgentHealthDecreased()
        {
            if (canBeStunned && agent.health.CurrentPoints > 0)
            {
                Stun();
            }
        }
    }
}