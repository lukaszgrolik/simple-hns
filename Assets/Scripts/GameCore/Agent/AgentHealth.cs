using UnityEngine;

namespace GameCore
{
    public class AgentHealth : AgentComponent
    {
        // @todo magic number
        private int maxPoints = 100;
        public int MaxPoints => maxPoints;

        private int currentPoints;
        public int CurrentPoints => currentPoints;

        public bool isAlive => currentPoints > 0;
        public bool isDead => isAlive == false;

        public event System.Action<GameCore.Agent> healthChanged;

        public event System.Action<Agent> died;

        public AgentHealth()
        {
            currentPoints = maxPoints;
        }

        public void TakeDamage(int damagePoints)
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