using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class Item
    {
        public readonly DataDefinition.Item itemData;
        private Vector2 size = Vector2.zero;

        public Item(DataDefinition.Item itemData)
        {
            this.itemData = itemData;
        }
    }

    public class ItemSystem
    {
        public readonly List<DroppedItem> droppedItems = new List<DroppedItem>();

        public event System.Action<DroppedItem, Vector3> itemSpawned;
        public event System.Action<DroppedItem> itemDeleted;

        public void Drop(Game game, Item item, Vector3 pos)
        {
            var droppedItem = new DroppedItem(
                game: game,
                item: item
            );

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