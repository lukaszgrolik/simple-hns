using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AgentCardEquipment
    {
        private AgentCard agentCard;
        private AgentEquipment agentEquipment;

        public event System.Action combinedAttrsChanged;

        public AgentCardEquipment(
            AgentCard agentCard,
            AgentEquipment agentEquipment
        )
        {
            this.agentCard = agentCard;
            this.agentEquipment = agentEquipment;

            agentEquipment.itemEquipped += OnItemEquipped;
            agentEquipment.itemUnequipped += OnItemUnequipped;
        }

        void OnItemEquipped(Item item)
        {
            // Debug.Log($"OnItemEquipped: {item}");

            UpdateCombinedAttrs();
        }

        void OnItemUnequipped(Item item)
        {
            UpdateCombinedAttrs();
        }

        void UpdateCombinedAttrs()
        {
            agentCard.combinedAttrsCollection.Replace(AgentAttributesCollection.Sum(agentCard.attrsCollection, agentEquipment.wornItemsAttrsCollection));

            combinedAttrsChanged?.Invoke();
        }
    }

    public class AgentCard
    {
        private float life;

        public readonly AgentAttributesCollection attrsCollection = new AgentAttributesCollection();
        public readonly AgentAttributesCollection combinedAttrsCollection = new AgentAttributesCollection();
    }
}