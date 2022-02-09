using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonoBehaviors
{
    public class Projectile : MonoBehaviour
    {
        AgentController agentController;

        public void Setup(AgentController agentController)
        {
            this.agentController = agentController;

            var rb = GetComponent<Rigidbody>();
            rb.velocity = transform.forward * 15;

            Destroy(gameObject, 3);
        }

        void OnTriggerEnter(Collider info)
        {
            // if (agentController.AliveEnemies.TryGetValue(info.gameObject, out var agent))
            // {
            //     agent.Health.TakeDamage(10);

            //     Destroy(gameObject);
            // }
        }
    }
}
