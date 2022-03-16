using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviors
{
    class GameplayManagerProjectileEntities
    {
        // private Dictionary<ProjectileType, DataDefinition.Skill_CastProjectile> dict_projectileType_projectileData = new Dictionary<ProjectileType, DataDefinition.Skill_CastProjectile>()
        // {
        //     [ProjectileType.FireBolt] = DataInstance.Skills.FireBolt,
        //     [ProjectileType.FireBall] = DataInstance.Skills.FireBall,
        //     [ProjectileType.EnergyBolt] = DataInstance.Skills.EnergyBolt,
        // };
        // private Dictionary<DataDefinition.Skill_CastProjectile, ProjectileType> dict_projectileData_projectileType = new Dictionary<DataDefinition.Skill_CastProjectile, ProjectileType>();

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
            // var projectileType = dict_projectileData_projectileType[projectile.projectileSkillData];
            var projectileType = gameplayManager.DataStore.projectileTypes[projectile.projectileSkillData];
            var (projectileObject, projectileMB) = InstantiateProjectile(projectile, projectileType, pos, rot);

            // dict_object_transformScript.Add(projectileObject, projectile);
            gameplayManager.dict_transformScript_object.Add(projectile, projectileObject);
        }

        (GameObject, Projectile) InstantiateProjectile(
            GameCore.Projectile projectile,
            ProjectileType projectileType,
            Vector3 pos,
            Quaternion rot
        )
        {
            var projectileObject = GameObject.Instantiate(projectilePrefab, pos, rot, projectilesContainer);
            var projectileMB = projectileObject.GetComponent<Projectile>();

            projectileMB.Setup(
                gameplayManager: gameplayManager,
                // originatorAgentCtrl: combat.AgentController,
                projectile: projectile
            );

            var projectileModelPrefab = gameplayManager.FindProjectileModelPrefab(projectileType);
            var projectileModelObj = GameObject.Instantiate(projectileModelPrefab, projectileMB.transform);

            var spriteModel = projectileModelObj.GetComponentInChildren<SpriteModel>();
            if (spriteModel == null) throw new System.Exception($"projectile SpriteModel not found: {projectileType}");

            // @todo magic number
            spriteModel.transform.rotation = Quaternion.Euler(37.5f, 45f, 0f);

            return (projectileObject, projectileMB);
        }

        public void OnProjectileDeleted(GameCore.Projectile projectile)
        {
            var projectileObject = gameplayManager.dict_transformScript_object[projectile];

            GameObject.Destroy(projectileObject);
        }
    }
}
