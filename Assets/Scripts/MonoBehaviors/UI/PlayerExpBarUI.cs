using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviors
{
    public class PlayerExpBarUI : MonoBehaviour
    {
        [SerializeField] private Image progressBarImage;

        private GameCore.Agent agent;

        public void Setup()
        {

        }

        public void SetPlayer(GameCore.Agent agent)
        {
            this.agent = agent;

            OnAgentExpChanged(agent);

            agent.agentLevel.expChanged += OnAgentExpChanged;
            agent.agentLevel.levelledUp += OnAgentLevelledUp;
        }

        void OnAgentExpChanged(GameCore.Agent agent)
        {
            // Debug.Log($"{agent.agentLevel.CurrentExp} / {agent.agentLevel.NextLevelRequiredExp} (current level: {agent.agentLevel.CurrentLevel})");
            // progressBarImage.fillAmount = (float)agent.agentLevel.CurrentExp / agent.agentLevel.NextLevelRequiredExp;
            progressBarImage.fillAmount = agent.agentLevel.CurrentLevelExpProgress;
        }

        void OnAgentLevelledUp(GameCore.Agent agent)
        {
            if (agent.agentLevel.CurrentLevel == agent.game.agentLeveling.MAX_LEVEL)
            {
                progressBarImage.fillAmount = 0;

                agent.agentLevel.expChanged -= OnAgentExpChanged;
                agent.agentLevel.levelledUp -= OnAgentLevelledUp;
            }
        }
    }
}