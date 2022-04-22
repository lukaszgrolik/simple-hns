using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviors
{
    public class Game : GameCore.Game
    {
        private GameplayManager gameplayManager;

        public Game(
            GameplayManager gameplayManager
        )
        {
            this.gameplayManager = gameplayManager;
        }

        public override Vector3 GetPosition(GameCore.ITransformScript script)
        {
            return gameplayManager.dict_transformScript_object[script].transform.position;
        }

        public override Vector3 GetProjectileSpawnPosition(GameCore.Agent agent)
        {
            var agentCtrl = gameplayManager.dict_agent_agentCtrl[agent];
            // var projSpawnPoint = agentCtrl.GetComponentInChildren<AgentProjectileSpawnPoint>();
            var projSpawnPos = agentCtrl.transform.position + Vector3.up * 1f;

            // return projSpawnPoint.transform.position;
            return projSpawnPos;
        }

        public override List<GameCore.Agent> FindAgentsInRadius(Vector3 pos, float radius)
        {
            var agents = new List<GameCore.Agent>();
            var agentControllers = Utils.FindColliders<AgentController>(pos, radius, gameplayManager.AgentLayerMask);

            for (int i = 0; i < agentControllers.Count; i++)
            {
                agents.Add(agentControllers[i].Agent);
            }

            return agents;
        }
    }
}
