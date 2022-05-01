using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class Item
    {
        public readonly DataDefinition.Item itemData;
        private Vector2 size = Vector2.zero;

        public readonly AgentAttributesCollection attrsCollection;

        public Item(
            DataDefinition.Item itemData,
            List<AgentAttribute> attrs = null
        )
        {
            this.itemData = itemData;
            this.attrsCollection = new AgentAttributesCollection(attrs);
        }
    }

    public class ItemSystem
    {
        private Game game;

        public readonly List<DroppedItem> droppedItems = new List<DroppedItem>();

        public event System.Action<DroppedItem, Vector3> itemSpawned;
        public event System.Action<DroppedItem> itemDeleted;

        public ItemSystem(Game game)
        {
            this.game = game;
        }

        public void Drop(Item item, Vector3 pos)
        {
            var droppedItem = new DroppedItem(
                game: game,
                item: item
            );

            Debug.Log($"item dropped: {item.attrsCollection.attributes.Count} attrs - {item.attrsCollection.ToString()}");

            droppedItems.Add(droppedItem);

            itemSpawned?.Invoke(droppedItem, pos);
        }

        public void Delete(DroppedItem droppedItem)
        {
            droppedItems.Remove(droppedItem);

            itemDeleted?.Invoke(droppedItem);
        }
    }
}