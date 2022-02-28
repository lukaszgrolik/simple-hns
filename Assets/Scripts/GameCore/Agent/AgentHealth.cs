using UnityEngine;

namespace GameCore
{
    public class AgentHealth : AgentComponent
    {
        // @todo magic number
        int maxPoints = 100;
        public int MaxPoints => maxPoints;

        int currentPoints;
        public int CurrentPoints => currentPoints;

        public bool isAlive => currentPoints > 0;
        public bool isDead => isAlive == false;

        public event System.Action<int, int> healthChanged;

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

            healthChanged?.Invoke(currentPoints, maxPoints);

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