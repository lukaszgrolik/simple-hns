using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public abstract class AgentAttribute
    {
        // PlusLife,
        // EnhancedDamage,

        public abstract string Label { get; }

        protected int value;
        public int Value => value;

        public AgentAttribute(int value)
        {
            this.value = value;
        }

        public abstract AgentAttribute Clone();

        public void Increment(AgentAttribute agentAttribute)
        {
            this.value += agentAttribute.value;
        }

        public void Decrement(AgentAttribute agentAttribute)
        {
            this.value -= agentAttribute.value;
        }
    }

    // public abstract class AgentAttribute_IntValue : AgentAttribute
    // {

    // }

    public class AgentAttribute_PlusLife : AgentAttribute
    {
        public override string Label => "life";

        public AgentAttribute_PlusLife(int value) : base(value) { }

        public override AgentAttribute Clone() { return new AgentAttribute_PlusLife(value); }
    }

    public class AgentAttribute_IncreasedMovementSpeed : AgentAttribute
    {
        public override string Label => "increased movement speed";

        public AgentAttribute_IncreasedMovementSpeed(int value) : base(value) { }

        public override AgentAttribute Clone() { return new AgentAttribute_IncreasedMovementSpeed(value); }
    }

    public class AgentAttribute_IncreasedAttackRate : AgentAttribute
    {
        public override string Label => "increased attack rate";

        public AgentAttribute_IncreasedAttackRate(int value) : base(value) { }

        public override AgentAttribute Clone() { return new AgentAttribute_IncreasedAttackRate(value); }
    }

    public class AgentAttribute_EnhancedDamage : AgentAttribute
    {
        public override string Label => "enhanced damage";

        public AgentAttribute_EnhancedDamage(int value) : base(value) { }

        public override AgentAttribute Clone() { return new AgentAttribute_EnhancedDamage(value); }
    }

    public class AgentAttribute_LifeRegen : AgentAttribute
    {
        public override string Label => "life regeneration";

        public AgentAttribute_LifeRegen(int value) : base(value) { }

        public override AgentAttribute Clone() { return new AgentAttribute_LifeRegen(value); }
    }

    public class AgentAttribute_LifeStolenPerHit : AgentAttribute
    {
        public override string Label => "life stolen per hit";

        public AgentAttribute_LifeStolenPerHit(int value) : base(value) { }

        public override AgentAttribute Clone() { return new AgentAttribute_LifeStolenPerHit(value); }
    }

    public class AgentAttribute_LifeStolenPerKill : AgentAttribute
    {
        public override string Label => "life stolen per kill";

        public AgentAttribute_LifeStolenPerKill(int value) : base(value) { }

        public override AgentAttribute Clone() { return new AgentAttribute_LifeStolenPerKill(value); }
    }
}