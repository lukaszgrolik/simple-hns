using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MonoBehaviors
{
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

        public void ShowEnemyHealth(string name, float currentPoints, float maxPoints)
        {
            enemyNameText.text = name;
            UpdateEnemyHealth(currentPoints, maxPoints);

            enemyHealthObject.SetActive(true);
        }

        public void HideEnemyHealth()
        {
            enemyHealthObject.SetActive(false);
        }

        public void UpdateEnemyHealth(float currentPoints, float maxPoints)
        {
            enemyHealthImage.fillAmount = currentPoints / maxPoints;
        }
    }
}
