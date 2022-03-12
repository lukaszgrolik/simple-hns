using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MonoBehaviors
{
    public class QuestsListItemUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text statusText;
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;

        public void Setup(GameCore.Quest quest)
        {
            statusText.text = quest.Status.ToString();
            titleText.text = quest.Title;
            descriptionText.text = quest.Description;
        }

        public void UpdateQuest(GameCore.Quest quest)
        {

        }
    }
}