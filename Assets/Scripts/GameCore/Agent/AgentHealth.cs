using UnityEngine;

namespace GameCore
{
    // x; //
    // public class AgentHealthRegen
    // {

    // }

    public class AgentHealth : AgentComponent
    {
        private float maxPoints;
        public float MaxPoints => maxPoints;

        private float currentPoints;
        public float CurrentPoints => currentPoints;

        public bool isAlive => currentPoints > 0;
        public bool isDead => isAlive == false;

        private Agent killedBy;
        public Agent KilledBy => killedBy;

        public event System.Action healthDecreased;
        public event System.Action<GameCore.Agent> healthChanged;

        public event System.Action<Agent> died;

        public AgentHealth(float maxPoints)
        {
            this.maxPoints = maxPoints;

            currentPoints = GetTotalMaxPoints();
        }

        float GetTotalMaxPoints()
        {
            // x; // sum from agent card and equipped items
            // return maxPoints + agent.agentCard.plusLife + agent.agentEquipment.plusLife;
            return maxPoints;
        }

        public void TakeDamage(float damagePoints, Agent attacker)
        {
            currentPoints -= damagePoints;
            if (currentPoints < 0) currentPoints = 0;
            // Debug.Log($"took damage: {damagePoints} ({currentPoints}/{maxPoints})");

            healthDecreased?.Invoke();
            healthChanged?.Invoke(agent);

            if (currentPoints == 0)
            {
                Die(attacker);
            }
        }

        public void Heal()
        {
            currentPoints = GetTotalMaxPoints();

            healthChanged?.Invoke(agent);
        }

        void Die(Agent attacker)
        {
            killedBy = attacker;

            died?.Invoke(agent);
        }
    }
}