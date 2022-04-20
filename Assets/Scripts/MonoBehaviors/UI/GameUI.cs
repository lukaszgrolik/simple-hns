using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

namespace MonoBehaviors
{
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

        private PlayerExpBarUI playerExpBarUI;
        public PlayerExpBarUI PlayerExpBarUI => playerExpBarUI;

        [Header("Enemy health")]
        [SerializeField] private GameObject enemyHealthObject;
        [SerializeField] private TMP_Text enemyNameText;
        [SerializeField] private Image enemyHealthImage;

        [Header("Player death")]
        [SerializeField] private GameObject playerDeathPanel;
        [SerializeField] private Button restartButton;

        private CharacterCardUI characterCardUI;
        public CharacterCardUI CharacterCardUI => characterCardUI;

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

            this.playerExpBarUI = GetComponentInChildren<PlayerExpBarUI>();
            playerExpBarUI.Setup();

            this.enemyHealthUI = new EnemyHealthUI(
                enemyHealthObject,
                enemyNameText,
                enemyHealthImage
            );
            this.playerDeathUI = new PlayerDeathUI(
                restartButton
            );

            this.characterCardUI = GetComponentInChildren<CharacterCardUI>(includeInactive: true);
            characterCardUI.Setup();

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

        public void ToggleCharacterCardPanel()
        {
            characterCardUI.Toggle();
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
