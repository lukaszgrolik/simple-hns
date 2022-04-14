using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AgentDrop
    {
        public AgentDrop(AgentHealth health)
        {
            health.died += OnAgentDied;
        }

        void OnAgentDied(Agent agent)
        {
            // DropItems(agent);
        }

        void DropItems(Agent agent)
        {
            var game = agent.game;

            var itemDataList = new List<DataDefinition.Item>(){
                DataInstance.Items.handAxe,
                DataInstance.Items.shortStaff,
                DataInstance.Items.shortSword,
                DataInstance.Items.skullCap,
                DataInstance.Items.smallShield,
            };
            var itemData = itemDataList.Random();
            var item = new Item(
                itemData: itemData
            );

            var circlePos = Random.insideUnitCircle * 1;
            var pos = game.GetPosition(agent) + new Vector3(circlePos.x, 0, circlePos.y);

            game.itemSystem.Drop(game, item, pos);
        }
    }
}