using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace MonoBehaviors
{
    public class PlayerHealthUI
    {
        private Image playerHealthImage;
        private TMP_Text playerHealthText;

        public PlayerHealthUI(
            Image playerHealthImage,
            TMP_Text playerHealthText
        )
        {
            this.playerHealthImage = playerHealthImage;
            this.playerHealthText = playerHealthText;
        }

        public void SetPlayerHealth(int currentPoints, int maxPoints)
        {
            playerHealthImage.fillAmount = (float)currentPoints / maxPoints;
            playerHealthText.text = $"{currentPoints}/{maxPoints}";
        }
    }

    public class EnemyHealthUI
    {
        private GameObject enemyHealthObject;
        private TMP_Text enemyNameText;
        private Image enemyHealthImage;

        public EnemyHealthUI(
            GameObject enemyHealthObject,
            TMP_Text enemyNameText,
            Image enemyHealthImage
        )
        {
            this.enemyHealthObject = enemyHealthObject;
            this.enemyNameText = enemyNameText;
            this.enemyHealthImage = enemyHealthImage;
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
    }

    public class PlayerDeathUI
    {
        public PlayerDeathUI(
            Button restartButton
        )
        {
            restartButton.onClick.AddListener(OnRestartButtonClick);
        }

        void OnRestartButtonClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public class GameUI : MonoBehaviour
    {
        private GameplayManager gameplayManager;

        private PlayerHealthUI playerHealthUI;
        public PlayerHealthUI PlayerHealthUI => playerHealthUI;

        private EnemyHealthUI enemyHealthUI;
        public EnemyHealthUI EnemyHealthUI => enemyHealthUI;

        private PlayerDeathUI playerDeathUI;
        private QuestsUI questsUI;
        private EquipmentUI equipmentUI;

        [Header("Player health")]
        [SerializeField] private Image playerHealthImage;
        [SerializeField] private TMP_Text playerHealthText;

        [Header("Enemy health")]
        [SerializeField] private GameObject enemyHealthObject;
        [SerializeField] private TMP_Text enemyNameText;
        [SerializeField] private Image enemyHealthImage;

        [Header("Player death")]
        [SerializeField] private GameObject playerDeathPanel;
        [SerializeField] private Button restartButton;

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

            this.playerHealthUI = new PlayerHealthUI(
                playerHealthImage,
                playerHealthText
            );
            this.enemyHealthUI = new EnemyHealthUI(
                enemyHealthObject,
                enemyNameText,
                enemyHealthImage
            );
            this.playerDeathUI = new PlayerDeathUI(
                restartButton
            );
            this.questsUI = new QuestsUI(
                questItemPrefab,
                questsContainer
            );
            this.equipmentUI = new EquipmentUI(
                gameplayManager,
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

        public void ShowPlayerDeathPanel()
        {
            playerDeathPanel.SetActive(true);
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

                equipmentUI.SetWornItems(gameplayManager.ControlledAgent.Agent.equipment.wornItems);
                equipmentUI.SetCarriedItems(gameplayManager.ControlledAgent.Agent.equipment.carriedItems);
            }
        }
    }

}
