using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class Projectile : ITransformScript
    {
        private Game game;
        private Agent originatorAgent;

        // public event System.Action disappeared;

        private float timeElapsed = 0;

        public Projectile(
            Game game,
            Agent originatorAgent
        )
        {
            this.game = game;
            this.originatorAgent = originatorAgent;
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
                otherAgent.health.TakeDamage(10);
                // Debug.Log($"health: {otherAgent.health.CurrentPoints}");

                Disappear();
            }
        }

        void Disappear()
        {
            // disappeared?.Invoke();
            game.DeleteProjectile(this);
        }

        void Explode()
        {

        }
    }
}