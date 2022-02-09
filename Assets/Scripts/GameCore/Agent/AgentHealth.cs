using UnityEngine;
using UnityEngine.Events;

namespace GameCore
{
    public class AgentHealth
    {
        int maxPoints = 100;
        public int MaxPoints => maxPoints;

        int currentPoints = 100;
        public int CurrentPoints => currentPoints;

        public class HealthChangedEvent : UnityEvent<int, int> { }
        public readonly HealthChangedEvent healthChanged = new HealthChangedEvent();

        public class DiedEvent : UnityEvent<AgentHealth> { }
        public readonly DiedEvent died = new DiedEvent();

        public AgentHealth()
        {

        }

        public void TakeDamage(int damagePoints)
        {
            currentPoints -= damagePoints;
            if (currentPoints < 0) currentPoints = 0;

            healthChanged.Invoke(currentPoints, maxPoints);

            if (currentPoints == 0)
            {
                Die();
            }
        }

        void Die()
        {
            died.Invoke(this);
        }
    }
}