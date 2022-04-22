using System.Collections;
using System.Collections.Generic;

namespace DataDefinition
{
    public class AgentParty
    {
        public readonly string name;

        public AgentParty(
            string name
        )
        {
            this.name = name;
        }
    }

    public class Agent
    {
        public readonly string name;

        public readonly float health;
        public readonly float healthDeviation;

        public readonly bool canBeStunned;
        public readonly float stunTime;

        public readonly float walkingSpeed;
        public readonly float runningSpeed;
        public readonly float speedDeviation;

        public readonly float sightRadius;
        public readonly float sightRadiusDeviation;

        public readonly List<Skill> skills;
        public readonly float attackRate;

        public Agent(
            string name,

            float health = 10f,
            float healthDeviation = 0,

            bool canBeStunned = true,
            float stunTime = 1f,

            float walkingSpeed = 3f,
            float runningSpeed = 6f,
            float speedDeviation = 0f,

            float sightRadius = 10f,
            float sightRadiusDeviation = 0f,

            List<Skill> skills = null,
            float attackRate = 1f
        )
        {
            this.name = name;

            this.health = health;
            this.healthDeviation = healthDeviation;

            this.canBeStunned = canBeStunned;
            this.stunTime = stunTime;

            this.walkingSpeed = walkingSpeed;
            this.runningSpeed = runningSpeed;
            this.speedDeviation = speedDeviation;

            this.sightRadius = sightRadius;
            this.sightRadiusDeviation = sightRadiusDeviation;

            this.skills = skills != null ? skills : new List<Skill>();
            this.attackRate = attackRate;
        }
    }
}