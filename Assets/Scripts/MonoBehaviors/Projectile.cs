using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonoBehaviors
{
    public class Projectile : MonoBehaviour
    {
        private AgentController originatorAgentCtrl;
        private GameCore.Projectile projectile;

        public void Setup(
            AgentController originatorAgentCtrl,
            GameCore.Projectile projectile
        )
        {
            this.originatorAgentCtrl = originatorAgentCtrl;
            this.projectile = projectile;

            var rb = GetComponent<Rigidbody>();
            rb.velocity = transform.forward * 15;

            Destroy(gameObject, 3);
        }

        void OnTriggerEnter(Collider coll)
        {
            // disappear/explode on prop and enemy contact (go through allies and other projectiles)
            // damage enemy

            // agent.AliveEnemies.Contains(otherAgent);
            if (originatorAgentCtrl.AliveEnemies.TryGetValue(coll.gameObject, out var agent))
            {
                projectile.OnCollidedWithEnemy(otherAgent);

                Destroy(gameObject);
            }

            // projectile.OnCollidedWithProp();
        }
    }
}
