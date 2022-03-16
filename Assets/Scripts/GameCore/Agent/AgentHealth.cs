using UnityEngine;

namespace GameCore
{
    public class AgentHealth : AgentComponent
    {
        private float maxPoints;
        public float MaxPoints => maxPoints;

        private float currentPoints;
        public float CurrentPoints => currentPoints;

        public bool isAlive => currentPoints > 0;
        public bool isDead => isAlive == false;

        public event System.Action<GameCore.Agent> healthChanged;

        public event System.Action<Agent> died;

        public AgentHealth(float maxPoints)
        {
            this.maxPoints = maxPoints;

            currentPoints = maxPoints;
        }

        public void TakeDamage(float damagePoints)
        {
            currentPoints -= damagePoints;
            if (currentPoints < 0) currentPoints = 0;
            // Debug.Log($"took damage: {damagePoints} ({currentPoints}/{maxPoints})");

            healthChanged?.Invoke(agent);

            if (currentPoints == 0)
            {
                Die();
            }
        }

        void Die()
        {
            died?.Invoke(agent);
        }
    }
}