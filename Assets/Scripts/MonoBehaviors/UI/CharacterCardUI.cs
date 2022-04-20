using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MonoBehaviors
{
    public class CharacterCardUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text charNameText;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text attrsText;

        private GameCore.Agent agent;

        public void Setup()
        {
            gameObject.SetActive(false);
        }

        public void SetPlayer(GameCore.Agent agent)
        {
            this.agent = agent;

            OnAgentExpChanged(agent);
            OnAgentCombinedAttrsChanged();

            agent.agentLevel.expChanged += OnAgentExpChanged;
            agent.agentCardEquipment.combinedAttrsChanged += OnAgentCombinedAttrsChanged;
        }

        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        void OnAgentExpChanged(GameCore.Agent agent)
        {
            levelText.text = $"Level: {agent.agentLevel.CurrentLevel} | Experience: {agent.agentLevel.CurrentExp}";
        }

        void OnAgentCombinedAttrsChanged()
        {
            var lines = new List<string>();

            for (int i = 0; i < agent.agentCard.combinedAttrsCollection.attributes.Count; i++)
            {
                var attr = agent.agentCard.combinedAttrsCollection.attributes[i];

                lines.Add($"{attr.Label}: {attr.Value}");
            }

            attrsText.text = lines.Count == 0 ? "(no attributes)" : string.Join("\n", lines);
        }
    }
}