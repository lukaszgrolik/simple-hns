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

        private CameraFollow cameraFollow;
        public CameraFollow CameraFollow => cameraFollow;

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

        public void Setup()
        {
            this.cameraFollow = gameplayManager.GetComponent<CameraFollow>();

            this.gameUI.Setup(gameplayManager);
        }

        // can only be called after controlledAgent is set
        public void Init()
        {
            playerController = gameplayManager.GetComponent<PlayerController>();
            playerController.Setup(gameplayManager, this);

            playerController.MouseHover.agentMouseEntered += OnPlayerAgentMouseEntered;
            playerController.MouseHover.agentMouseLeft += OnPlayerAgentMouseLeft;

            cameraFollow.Setup(gameplayManager, controlledAgent.transform);

            var ctrlAgentHealth = controlledAgent.Agent.health;

            gameUI.PlayerHealthUI.SetPlayerHealth(ctrlAgentHealth.CurrentPoints, ctrlAgentHealth.MaxPoints);
            gameUI.EnemyHealthUI.HideEnemyHealth();

            ctrlAgentHealth.healthChanged += OnControlledAgentHealthPointsChanged;
            ctrlAgentHealth.died += OnControlledAgentDied;

            controlledAgent.SetRunningMode();
        }

        void OnControlledAgentHealthPointsChanged(GameCore.Agent agent)
        {
            gameUI.PlayerHealthUI.SetPlayerHealth(agent.health.CurrentPoints, agent.health.MaxPoints);
        }

        void OnControlledAgentDied(GameCore.Agent agent)
        {
            gameUI.ShowPlayerDeathPanel();
        }

        void OnPlayerAgentMouseEntered(GameCore.Agent agent)
        {
            if (controlledAgent.Agent.partyMember.AgentsParty.IsAliveEnemy(agent))
            {
                gameUI.EnemyHealthUI.ShowEnemyHealth(agent.agentData.name, agent.health.CurrentPoints, agent.health.MaxPoints);

                agent.health.healthChanged += OnPlayerHoveredAgentHealthChanged;
            }
        }

        void OnPlayerAgentMouseLeft(GameCore.Agent agent)
        {
            gameUI.EnemyHealthUI.HideEnemyHealth();

            agent.health.healthChanged -= OnPlayerHoveredAgentHealthChanged;
        }

        void OnPlayerHoveredAgentHealthChanged(GameCore.Agent agent)
        {
            if (agent.health.CurrentPoints == 0)
            {
                gameUI.EnemyHealthUI.HideEnemyHealth();

                agent.health.healthChanged -= OnPlayerHoveredAgentHealthChanged;
            }
            else
            {
                gameUI.EnemyHealthUI.UpdateEnemyHealth(agent.health.CurrentPoints, agent.health.MaxPoints);
            }
        }
    }
}
