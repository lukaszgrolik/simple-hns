using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MonoBehaviors
{
    public class EquipmentUI
    {
        private GameplayManager gameplayManager;

        private GameObject wornItemPrefab;
        private GameObject unwornItemPrefab;
        private Image helmImage;
        private Image handRightImage;
        private Image handLeftImage;
        private Transform unwornItemsContainer;

        public EquipmentUI(
            GameplayManager gameplayManager,

            GameObject wornItemPrefab,
            GameObject unwornItemPrefab,
            Image helmImage,
            Image handRightImage,
            Image handLeftImage,
            Transform unwornItemsContainer
        )
        {
            this.gameplayManager = gameplayManager;

            this.wornItemPrefab = wornItemPrefab;
            this.unwornItemPrefab = unwornItemPrefab;
            this.helmImage = helmImage;
            this.handRightImage = handRightImage;
            this.handLeftImage = handLeftImage;
            this.unwornItemsContainer = unwornItemsContainer;
        }

        public void Setup()
        {
            SetHelmImage(null);
            SetHandRightImage(null);
            SetHandLeftImage(null);
        }

        public void SetWornItems(Dictionary<GameCore.AgentEquipment.WornItemSlot, GameCore.Item> items)
        {

        }

        public void SetCarriedItems(List<GameCore.Item> items)
        {
            foreach (var item in items)
            {
                var itemObj = Object.Instantiate(unwornItemPrefab, unwornItemsContainer);
                var itemMB = itemObj.GetComponent<EquipmentCarriedItemsListItemUI>();

                itemMB.Setup();
            }
        }

        void SetHelmImage(Image image)
        {

        }

        void SetHandRightImage(Image image)
        {


        }

        void SetHandLeftImage(Image image)
        {

        }

        void AddUnwornItem()
        {

        }

        void RemoveUnwornItem(int index)
        {

        }
    }
}
