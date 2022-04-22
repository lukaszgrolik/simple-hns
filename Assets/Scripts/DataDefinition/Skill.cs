using System.Collections;
using System.Collections.Generic;

namespace DataDefinition
{
    public abstract class Skill
    {
        public readonly string name;

        public Skill(string name)
        {
            this.name = name;
        }
    }

    public class Skill_Custom : Skill
    {
        public Skill_Custom(
            string name
        ) : base(name)
        {

        }
    }
    public class Skill_Melee : Skill
    {
        public readonly float damage;
        public readonly float damageDeviation;
        public readonly float angle;

        public Skill_Melee(
            string name,
            float damage = 10,
            float damageDeviation = 0,
            float angle = 0
        ) : base(name)
        {
            this.damage = damage;
            this.damageDeviation = damageDeviation;
            this.angle = angle;
        }
    }

    public class Skill_Bow : Skill
    {
        public readonly float damage = 10;
        public readonly float damageDeviation = 0;

        public Skill_Bow(
            string name,
            float damage = 10,
            float damageDeviation = 0
        ) : base(name)
        {
            this.damage = damage;
            this.damageDeviation = damageDeviation;
        }
    }

    public class Skill_CastProjectile : Skill
    {
        public readonly float speed;
        public readonly float damage;
        public readonly float damageDeviation;
        public readonly float splashRadius;

        public Skill_CastProjectile(
            string name,
            float speed = 20,
            float damage = 10,
            float damageDeviation = 0,
            float splashRadius = 0
        ) : base(name)
        {
            this.speed = speed;
            this.damage = damage;
            this.damageDeviation = damageDeviation;
            this.splashRadius = splashRadius;
        }
    }

    public class Skill_SummonAgent : Skill
    {
        public readonly Agent agent;
        public readonly int amount;

        public Skill_SummonAgent(
            string name,
            Agent agent,
            int amount = 1
        ) : base(name)
        {
            this.agent = agent;
            this.amount = amount;
        }
    }
}