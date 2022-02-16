using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore
{
    public class Projectile : ITransformScript
    {
        public void OnCollidedWithEnemy(Agent enemyAgent)
        {
            enemyAgent.health.TakeDamage(10);

            Disappear();
        }

        void Disappear()
        {
            // disappeared.Invoke()
        }

        void Explode()
        {

        }
    }
}