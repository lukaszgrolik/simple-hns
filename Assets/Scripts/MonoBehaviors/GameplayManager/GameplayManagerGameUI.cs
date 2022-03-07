using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviors
{
    public class GameplayManagerGameUI
    {
        private GameplayManager gameplayManager;
        private GameUI gameUI;
        private PlayerController playerController;

        private AgentController controlledAgent;

        public GameplayManagerGameUI(
            GameplayManager gameplayManager,
            GameUI gameUI
        )
        {
            this.gameplayManager = gameplayManager;
            this.gameUI = gameUI;
        }

        public void SetControlledAgent(AgentController agentController)
        {
            this.controlledAgent = agentController;
        }

        // can only be called after controlledAgent is set
        public void Init()
        {
            playerController = gameplayManager.GetComponent<PlayerController>();
            playerController.Setup(gameplayManager);

            playerController.MouseHover.agentMouseEntered += OnPlayerAgentMouseEntered;
            playerController.MouseHover.agentMouseLeft += OnPlayerAgentMouseLeft;

            var cameraFollow = gameplayManager.GetComponent<CameraFollow>();
            cameraFollow.Setup(gameplayManager, controlledAgent.transform);

            var ctrlAgentHealth = controlledAgent.Agent.health;

            gameUI.SetPlayerHealth(ctrlAgentHealth.CurrentPoints, ctrlAgentHealth.MaxPoints);
            gameUI.HideEnemyHealth();

            ctrlAgentHealth.healthChanged += OnControlledAgentHealthPointsChanged;
        }

        void OnControlledAgentHealthPointsChanged(GameCore.Agent agent)
        {
            gameUI.SetPlayerHealth(agent.health.CurrentPoints, agent.health.MaxPoints);
        }

        void OnPlayerAgentMouseEntered(GameCore.Agent agent)
        {
            if (controlledAgent.Agent.partyMember.AgentsParty.IsAliveEnemy(agent))
            {
                gameUI.ShowEnemyHealth(agent.agentData.Name, agent.health.CurrentPoints, agent.health.MaxPoints);

                agent.health.healthChanged += OnPlayerHoveredAgentHealthChanged;
            }
        }

        void OnPlayerAgentMouseLeft(GameCore.Agent agent)
        {
            gameUI.HideEnemyHealth();

            agent.health.healthChanged -= OnPlayerHoveredAgentHealthChanged;
        }

        void OnPlayerHoveredAgentHealthChanged(GameCore.Agent agent)
        {
            if (agent.health.CurrentPoints == 0)
            {
                gameUI.HideEnemyHealth();

                agent.health.healthChanged -= OnPlayerHoveredAgentHealthChanged;
            }
            else
            {
                gameUI.UpdateEnemyHealth(agent.health.CurrentPoints, agent.health.MaxPoints);
            }
        }
    }
}
