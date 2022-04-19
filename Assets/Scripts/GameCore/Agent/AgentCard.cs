using System.Collections;
using System.Collections.Generic;

namespace GameCore
{
    public abstract class AgentAttribute
    {
        // PlusLife,
        // EnhancedDamage,
    }

    public class AgentAttribute_PlusLife : AgentAttribute
    {

    }
    public class AgentAttribute_IncreasedMovementSpeed : AgentAttribute
    {

    }

    public class AgentCard
    {
        private float life;

        private float plusLife;
        private float enhancedDamage;
        private float increasedAttackRate;
        // private float increasedCastRate;
        private float increasedMovementSpeed;
        private float lifeStolenPerHit;
        private float lifeStolenPerKill;
        private float lifeRegeneration;
        private float plusMagicFind;

        public readonly List<AgentAttribute> attributes = new List<AgentAttribute>();
    }
}