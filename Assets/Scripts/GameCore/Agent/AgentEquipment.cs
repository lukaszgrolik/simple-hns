using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AgentEquipment : AgentComponent
    {
        public enum WornItemSlot
        {
            Helm,
            HandRight,
            HandLeft,
        }

        public readonly Dictionary<WornItemSlot, Item> wornItems = new Dictionary<WornItemSlot, Item>();

        public readonly List<Item> carriedItems = new List<Item>();

        public event System.Action<DroppedItem> itemPicked;
        public event System.Action<Item> itemDropped;

        public void Drop(Item item)
        {
            // foreach (var wornItem in wornItems)
            // {
            //     if (item == wornItem.Value)
            //     {
            //         // @todo
            //     }
            // }

            if (carriedItems.Contains(item))
            {
                carriedItems.Remove(item);

                agent.game.itemSystem.Drop(agent.game, item, agent.game.GetPosition(agent));

                itemDropped?.Invoke(item);
            }
        }

        public void Pick(DroppedItem droppedItem)
        {
            agent.game.itemSystem.Delete(droppedItem);

            carriedItems.Add(droppedItem.item);

            itemPicked?.Invoke(droppedItem);
        }
    }
}