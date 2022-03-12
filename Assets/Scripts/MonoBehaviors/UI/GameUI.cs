using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MonoBehaviors
{
    public class PlayerHealthUI
    {

    }

    public class EnemyHealthUI
    {

    }

    public class QuestsUI
    {
        private GameObject questItemPrefab;
        private Transform questsContainer;

        public QuestsUI(
            GameObject questItemPrefab,
            Transform questsContainer
        )
        {
            this.questItemPrefab = questItemPrefab;
            this.questsContainer = questsContainer;
        }

        public void SetQuests(List<GameCore.Quest> quests)
        {
            foreach (var quest in quests)
            {
                var questItemObj = Object.Instantiate(questItemPrefab, questsContainer);
                var questItemMB = questItemObj.GetComponent<QuestsListItemUI>();

                questItemMB.Setup(quest);
            }
        }
    }

    public class EquipmentUI
    {
        private GameObject wornItemPrefab;
        private GameObject unwornItemPrefab;
        private Image helmImage;
        private Image handRightImage;
        private Image handLeftImage;
        private Transform unwornItemsContainer;

        public EquipmentUI(
            GameObject wornItemPrefab,
            GameObject unwornItemPrefab,
            Image helmImage,
            Image handRightImage,
            Image handLeftImage,
            Transform unwornItemsContainer
        )
        {
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

        public void SetHelmImage(Image image)
        {

        }

        public void SetHandRightImage(Image image)
        {


        }

        public void SetHandLeftImage(Image image)
        {

        }

        public void AddUnwornItem()
        {

        }

        public void RemoveUnwornItem(int index)
        {

        }
    }

    public class GameUI : MonoBehaviour
    {
        private GameplayManager gameplayManager;

        private QuestsUI questsUI;
        private EquipmentUI equipmentUI;

        [Header("Player health")]
        [SerializeField] private Image playerHealthImage;
        [SerializeField] private TMP_Text playerHealthText;

        [Header("Enemy health")]
        [SerializeField] private GameObject enemyHealthObject;
        [SerializeField] private TMP_Text enemyNameText;
        [SerializeField] private Image enemyHealthImage;

        [Header("Quests")]
        [SerializeField] private GameObject questsPanel;
        [SerializeField] private GameObject questItemPrefab;
        [SerializeField] private Transform questsContainer;

        [Header("Equipment")]
        [SerializeField] private GameObject equipmentPanel;
        [SerializeField] private GameObject wornItemPrefab;
        [SerializeField] private GameObject unwornItemPrefab;
        [SerializeField] private Image helmImage;
        [SerializeField] private Image handRightImage;
        [SerializeField] private Image handLeftImage;
        [SerializeField] private Transform unwornItemsContainer;

        public void Setup(GameplayManager gameplayManager)
        {
            this.gameplayManager = gameplayManager;

            this.questsUI = new QuestsUI(
                questItemPrefab,
                questsContainer
            );
            this.equipmentUI = new EquipmentUI(
                wornItemPrefab,
                unwornItemPrefab,
                helmImage,
                handRightImage,
                handLeftImage,
                unwornItemsContainer
            );

            equipmentUI.Setup();

            questsPanel.SetActive(false);
            equipmentPanel.SetActive(false);
        }

        public void SetPlayerHealth(int currentPoints, int maxPoints)
        {
            playerHealthImage.fillAmount = (float)currentPoints / maxPoints;
            playerHealthText.text = $"{currentPoints}/{maxPoints}";
        }

        public void ShowEnemyHealth(string name, int currentPoints, int maxPoints)
        {
            enemyNameText.text = name;
            UpdateEnemyHealth(currentPoints, maxPoints);

            enemyHealthObject.SetActive(true);
        }

        public void HideEnemyHealth()
        {
            enemyHealthObject.SetActive(false);
        }

        public void UpdateEnemyHealth(int currentPoints, int maxPoints)
        {
            enemyHealthImage.fillAmount = (float)currentPoints / maxPoints;
        }

        public void ToggleQuestsPanel()
        {
            questsPanel.SetActive(!questsPanel.activeSelf);

            if (questsPanel.activeSelf)
            {
                questsContainer.DestroyChildren();

                questsUI.SetQuests(gameplayManager.Game.questSystem.quests);
            }
        }

        public void ToggleEquipmentPanel()
        {
            equipmentPanel.SetActive(!equipmentPanel.activeSelf);

            if (equipmentPanel.activeSelf)
            {
                unwornItemsContainer.DestroyChildren();
            }
        }
    }

}
