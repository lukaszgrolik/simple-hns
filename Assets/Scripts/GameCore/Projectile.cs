using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class Projectile : ITransformScript
    {
        public readonly Game game;
        public readonly Agent originatorAgent;

        public readonly DataDefinition.Skill_CastProjectile projectileSkillData;

        // public event System.Action disappeared;

        private float timeElapsed = 0;

        public Projectile(
            Game game,
            Agent originatorAgent,

            DataDefinition.Skill_CastProjectile projectileSkillData
        )
        {
            this.game = game;
            this.originatorAgent = originatorAgent;

            this.projectileSkillData = projectileSkillData;
        }

        public void OnUpdate(float deltaTime)
        {
            // destory after 3 s;
            // Destroy(gameObject, 3);
            // Disappear();

            timeElapsed += deltaTime;
            // @todo magic number
            if (timeElapsed >= 3f)
            {
                Disappear();
            }
        }

        public void OnCollidedWithAgent(Agent otherAgent)
        {
            if (originatorAgent.partyMember.AgentsParty.IsAliveEnemy(otherAgent))
            {

                if (projectileSkillData.splashRadius > 0)
                {
                    DamageRadius();
                }
                else
                {
                    DamageTarget(otherAgent);
                }
                // Debug.Log($"health: {otherAgent.health.CurrentPoints}");

                Disappear();
            }
        }

        public void OnCollidedWithProp()
        {
            if (projectileSkillData.splashRadius > 0)
            {
                DamageRadius();
                Disappear();
            }
        }

        void Disappear()
        {
            // disappeared?.Invoke();
            game.DeleteProjectile(this);
        }

        void DamageTarget(Agent otherAgent)
        {
            var damage = Utils.RandomValueWithDeviation(projectileSkillData.damage, projectileSkillData.damageDeviation);

            otherAgent.health.TakeDamage(damage);
        }

        void DamageRadius()
        {
            var damage = Utils.RandomValueWithDeviation(projectileSkillData.damage, projectileSkillData.damageDeviation);
            var agents = game.FindAgentsInRadius(game.GetPosition(this), projectileSkillData.splashRadius);

            for (int i = 0; i < agents.Count; i++)
            {
                var otherAgent = agents[i];

                if (originatorAgent.partyMember.AgentsParty.IsAliveEnemy(otherAgent))
                {
                    agents[i].health.TakeDamage(damage);
                }
            }
        }
    }
}