using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AgentEquipment : AgentComponent
    {
        public readonly List<Item> items = new List<Item>();

        public void Drop(Item item)
        {

        }

        public void Pick(DroppedItem droppedItem)
        {
            items.Add(droppedItem.item);

            agent.game.itemSystem.Delete(droppedItem);
        }
    }
}