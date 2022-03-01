using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MonoBehaviors
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private Image playerHealthImage;
        [SerializeField] private TMP_Text playerHealthText;

        [SerializeField] private GameObject enemyHealthObject;
        [SerializeField] private TMP_Text enemyNameText;
        [SerializeField] private Image enemyHealthImage;

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
    }

}
