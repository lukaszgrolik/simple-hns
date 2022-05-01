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
            Test1,
            Test2,
        }

        public readonly Dictionary<WornItemSlot, Item> wornItems;
        public readonly List<Item> carriedItems;

        public readonly AgentAttributesCollection wornItemsAttrsCollection = new AgentAttributesCollection();

        public event System.Action<DroppedItem> itemPicked;
        public event System.Action<Item> itemDropped;
        public event System.Action<Item> itemEquipped;
        public event System.Action<Item> itemUnequipped;

        public AgentEquipment(
            Dictionary<WornItemSlot, Item> wornItems = null,
            List<Item> carriedItems = null
        )
        {
            this.wornItems = wornItems != null ? wornItems : new Dictionary<WornItemSlot, Item>();
            this.carriedItems = carriedItems != null ? carriedItems : new List<Item>();

            foreach (var wornItem in this.wornItems)
            {
                wornItemsAttrsCollection.Add(wornItem.Value.attrsCollection);
            }
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

        public bool IsSlotFree(WornItemSlot slot)
        {
            if (wornItems.TryGetValue(slot, out var equippedItem))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void EquipItem(WornItemSlot slot, Item item)
        {
            if (wornItems.TryGetValue(slot, out var equippedItem))
            {
                Debug.Log("slot is occupied");
            }
            else
            {
                wornItems.Add(slot, item);

                wornItemsAttrsCollection.Add(item.attrsCollection);

                itemEquipped?.Invoke(item);
            }
        }

        public void UnequipItem(WornItemSlot slot)
        {
            if (wornItems.TryGetValue(slot, out var item) == false)
            {
                Debug.Log("slot is unoccupied");
            }
            else
            {
                wornItems.Remove(slot);

                wornItemsAttrsCollection.Remove(item.attrsCollection);

                itemUnequipped?.Invoke(item);
            }
        }
    }
}