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

        private float plusLife = 0;
        private float enhancedDamage = 0;
        private float increasedAttackRate = 0;
        // private float increasedCastRate = 0;
        private float increasedMovementSpeed = 0;
        private float lifeStolenPerHit = 0;
        private float lifeStolenPerKill = 0;
        private float lifeRegeneration = 0;
        private float plusMagicFind = 0;

        public event System.Action<DroppedItem> itemPicked;
        public event System.Action<Item> itemDropped;

        public AgentEquipment()
        {
            // foreach (var wornItem in wornItems)
            // {
            //     x; // list all stats
            //     plusLife += wornItem.plusLife;
            // }
        }

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

        public void EquipItem(Item item)
        {
            // x; // list all stats
            // plusLife += item.plusLife;
        }

        public void UnequipItem(Item item)
        {
            // x; // list all stats
            // plusLife -= item.plusLife;
        }
    }
}