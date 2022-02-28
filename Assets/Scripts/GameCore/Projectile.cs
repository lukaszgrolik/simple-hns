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

        public bool IsAliveEnemy(Agent enemyAgent)
        {
            return originatorAgent.partyMember.AgentsParty.aliveEnemies.Contains(enemyAgent);
        }

        public void OnCollidedWithEnemy(Agent enemyAgent)
        {
            enemyAgent.health.TakeDamage(10);

            Disappear();
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