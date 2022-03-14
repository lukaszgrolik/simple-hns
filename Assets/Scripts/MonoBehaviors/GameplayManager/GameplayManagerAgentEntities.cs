using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviors
{
    class GameplayManagerAgentEntities
    {
        private GameplayManager gameplayManager;

        private GameObject agentPrefab;
        private Transform agentsContainer;
        private List<GameplayManager.AgentModelPrefab> agentModelPrefabs;

        private GameCore.AgentsParty heroParty;
        private GameCore.AgentsParty neutralParty;
        private GameCore.AgentsParty monsterParty;

        public GameplayManagerAgentEntities(
            GameplayManager gameplayManager,

            GameObject agentPrefab,
            Transform agentsContainer,
            List<GameplayManager.AgentModelPrefab> agentModelPrefabs,

            GameCore.AgentsParty heroParty,
            GameCore.AgentsParty neutralParty,
            GameCore.AgentsParty monsterParty
        )
        {
            this.gameplayManager = gameplayManager;

            this.agentPrefab = agentPrefab;
            this.agentsContainer = agentsContainer;
            this.agentModelPrefabs = agentModelPrefabs;

            this.heroParty = heroParty;
            this.neutralParty = neutralParty;
            this.monsterParty = monsterParty;
        }

        public GameCore.Agent CreateAgent(GameCore.Game game, AgentType agentType)
        {
            var dict_agentType_agentData = new Dictionary<AgentType, DataDefinition.Agent>()
            {
                [AgentType.Hero] = DataInstance.Agents.hero,
                [AgentType.Warden] = DataInstance.Agents.warden,
                [AgentType.Demon] = DataInstance.Agents.demon,
                [AgentType.HoodedWarrior] = DataInstance.Agents.hoodedWarrior,
            };
            var agentData = dict_agentType_agentData[agentType];

            GameCore.AgentsParty agentsParty = null;
            GameCore.AgentControl agentControl = null;
            if (agentType == AgentType.Hero)
            {
                agentsParty = heroParty;
                agentControl = new GameCore.AgentPlayerControl();
            }
            else if (agentType == AgentType.Warden)
            {
                agentsParty = neutralParty;
                agentControl = new GameCore.AgentWarriorAIControl();
            }
            else
            {
                agentsParty = monsterParty;
                agentControl = new GameCore.AgentWarriorAIControl();
            }

            var agent = game.CreateAgent(
                agentData: agentData,
                agentsParty: agentsParty,
                agentControl: agentControl
            );

            return agent;
        }

        public void OnAgentSpawned(GameCore.Agent agent, Vector3 pos, Quaternion rot)
        {
            AgentType agentType = AgentType.Hero;
            if (agent.agentData == DataInstance.Agents.hero) agentType = AgentType.Hero;
            else if (agent.agentData == DataInstance.Agents.warden) agentType = AgentType.Warden;
            else if (agent.agentData == DataInstance.Agents.demon) agentType = AgentType.Demon;
            else if (agent.agentData == DataInstance.Agents.hoodedWarrior) agentType = AgentType.HoodedWarrior;

            // var obj = Instantiate(AgentPrefab, pos, rot, agentsContainer);
            // var projectile = obj.GetComponent<Projectile>();
            // projectile.Setup(combat.AgentController);
            var (agentObj, agentCtrl, spriteModel) = InstantiateAgent(agent, agentType, pos, rot);

            //     agents.Add(agent);
            //     agentObjectsControllers.Add(agentObj, agentCtrl);

            // dict_object_transformScript.Add(agentObj, agent);
            gameplayManager.dict_transformScript_object.Add(agent, agentObj);
            gameplayManager.dict_object_agentCtrl.Add(agentObj, agentCtrl);
            gameplayManager.dict_agent_agentCtrl.Add(agent, agentCtrl);

            agent.Setup();
            agentCtrl.OnSpawned(spriteModel);
        }

        (GameObject, AgentController, SpriteModel) InstantiateAgent(
            // GameCore.Game game,
            GameCore.Agent agent,
            AgentType agentType,
            Vector3 pos,
            Quaternion rot
        )
        {
            var agentObj = GameObject.Instantiate(agentPrefab, pos, rot, agentsContainer);
            var agentCtrl = agentObj.GetComponent<AgentController>();

            agentCtrl.Setup(
                gameplayManager: gameplayManager,
                agent: agent
            );

            var agentModelPrefab = agentModelPrefabs.Find(p => p.AgentType == agentType);
            if (agentModelPrefab == null) throw new System.Exception($"agent prefab not found: {agentType}");

            var agentModelObj = GameObject.Instantiate(agentModelPrefab.Prefab, agentCtrl.transform);

            var spriteModel = agentModelObj.GetComponentInChildren<SpriteModel>();
            if (spriteModel == null) throw new System.Exception($"agent SpriteModel not found: {agentType}");

            // @todo magic number
            spriteModel.transform.rotation = Quaternion.Euler(37.5f, 45f, 0f);

            // @todo reassign values before removing collider and navMeshAgent

            var modelCollider = agentModelObj.GetComponent<Collider>();
            GameObject.Destroy(modelCollider);

            var modelNavMeshAgent = agentModelObj.GetComponent<NavMeshAgent>();
            GameObject.Destroy(modelNavMeshAgent);

            // GameObject.Destroy(spriteModel);

            return (agentObj, agentCtrl, spriteModel);
        }
    }
}
