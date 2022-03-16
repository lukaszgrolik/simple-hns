using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonoBehaviors
{
    public class Projectile : MonoBehaviour
    {
        private GameplayManager gameplayManager;
        // private AgentController originatorAgentCtrl;
        private GameCore.Projectile projectile;

        public void Setup(
            GameplayManager gameplayManager,
            // AgentController originatorAgentCtrl,
            GameCore.Projectile projectile
        )
        {
            this.gameplayManager = gameplayManager;
            // this.originatorAgentCtrl = originatorAgentCtrl;
            this.projectile = projectile;

            // projectile.disappeared += OnProjectileDisappeared;

            var rb = GetComponent<Rigidbody>();
            rb.velocity = transform.forward * projectile.projectileSkillData.speed;

        }

        void OnTriggerEnter(Collider coll)
        {
            // Debug.Log($"coll: {coll.name}");

            // disappear/explode on prop and enemy contact (go through allies and other projectiles)
            // damage enemy

            // agent.AliveEnemies.Contains(otherAgent);
            // if (originatorAgentCtrl.Agent.partyMember.AgentsParty.AliveEnemies.TryGetValue(coll.gameObject, out var agent))
            var isAgent = gameplayManager.dict_object_agentCtrl.TryGetValue(coll.gameObject, out var otherAgent);
            // Debug.Log($"isAgent: {isAgent}");

            if (isAgent)
            {
                projectile.OnCollidedWithAgent(otherAgent.Agent);
            }

            // projectile.OnCollidedWithProp();
        }

        // void OnProjectileDisappeared()
        // {

        // }
    }
}
