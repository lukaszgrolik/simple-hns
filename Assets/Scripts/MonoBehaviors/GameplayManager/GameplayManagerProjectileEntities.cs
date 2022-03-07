using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviors
{
    class GameplayManagerProjectileEntities
    {
        private GameplayManager gameplayManager;

        private GameObject projectilePrefab;
        private Transform projectilesContainer;

        public GameplayManagerProjectileEntities(
            GameplayManager gameplayManager,
            GameObject projectilePrefab,
            Transform projectilesContainer
        )
        {
            this.gameplayManager = gameplayManager;
            this.projectilePrefab = projectilePrefab;
            this.projectilesContainer = projectilesContainer;
        }

        public void OnProjectileSpawned(GameCore.Projectile projectile, Vector3 pos, Quaternion rot)
        {
            var projectileObject = GameObject.Instantiate(projectilePrefab, pos, rot, projectilesContainer);
            var projectileMB = projectileObject.GetComponent<Projectile>();
            projectileMB.Setup(
                gameplayManager: gameplayManager,
                // originatorAgentCtrl: combat.AgentController,
                projectile: projectile
            );

            // dict_object_transformScript.Add(projectileObject, projectile);
            gameplayManager.dict_transformScript_object.Add(projectile, projectileObject);
        }

        public void OnProjectileDeleted(GameCore.Projectile projectile)
        {
            var projectileObject = gameplayManager.dict_transformScript_object[projectile];

            GameObject.Destroy(projectileObject);
        }
    }
}
