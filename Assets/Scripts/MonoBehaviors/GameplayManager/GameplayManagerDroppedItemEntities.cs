using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviors
{
    class GameplayManagerDroppedItemEntities
    {
        private GameplayManager gameplayManager;

        private GameObject droppedItemPrefab;
        private Transform droppedItemsContainer;
        private List<GameplayManager.ItemModelPrefab> itemModelPrefabs;

        public GameplayManagerDroppedItemEntities(
            GameplayManager gameplayManager,

            GameObject droppedItemPrefab,
            Transform droppedItemsContainer,
            List<GameplayManager.ItemModelPrefab> itemModelPrefabs
        )
        {
            this.gameplayManager = gameplayManager;

            this.droppedItemPrefab = droppedItemPrefab;
            this.droppedItemsContainer = droppedItemsContainer;
            this.itemModelPrefabs = itemModelPrefabs;
        }

        public void OnDroppedItemSpawned(GameCore.DroppedItem droppedItem, Vector3 pos)
        {
            var itemTypes = new Dictionary<DataDefinition.Item, ItemType>()
            {
                [DataInstance.Items.shortSword] = ItemType.Sword,
                [DataInstance.Items.handAxe] = ItemType.Axe,
                [DataInstance.Items.shortStaff] = ItemType.Staff,
                [DataInstance.Items.skullCap] = ItemType.Helm,
                [DataInstance.Items.smallShield] = ItemType.Shield,
            };
            var itemType = itemTypes[droppedItem.item.itemData];
            var rot = Quaternion.identity;
            var (droppedItemObject, droppedItemMB) = InstantiateDroppedItem(droppedItem, itemType, pos, rot);

            gameplayManager.dict_transformScript_object.Add(droppedItem, droppedItemObject);
        }

        public void OnDroppedItemDeleted(GameCore.DroppedItem droppedItem)
        {
            var droppedItemObject = gameplayManager.dict_transformScript_object[droppedItem];

            GameObject.Destroy(droppedItemObject);
        }

        (GameObject, DroppedItemMB) InstantiateDroppedItem(
            GameCore.DroppedItem droppedItem,
            ItemType itemType,
            Vector3 pos,
            Quaternion rot
        )
        {
            var droppedItemObject = GameObject.Instantiate(droppedItemPrefab, pos, rot, droppedItemsContainer);
            var droppedItemMB = droppedItemObject.GetComponent<DroppedItemMB>();
            droppedItemMB.Setup(
                gameplayManager: gameplayManager,
                droppedItem: droppedItem
            );

            var droppedItemModelPrefab = itemModelPrefabs.Find(p => p.ItemType == itemType);
            if (droppedItemModelPrefab == null) throw new System.Exception($"droppedItem prefab not found: {itemType}");

            var droppedItemModelObj = GameObject.Instantiate(droppedItemModelPrefab.Prefab, droppedItemMB.transform);

            return (droppedItemObject, droppedItemMB);
        }
    }
}
