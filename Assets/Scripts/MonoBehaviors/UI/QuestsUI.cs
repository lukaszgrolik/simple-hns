using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MonoBehaviors
{
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
}
