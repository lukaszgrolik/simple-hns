using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        public void SetPlayerHealth(float currentPoints, float maxPoints)
        {
            playerHealthImage.fillAmount = currentPoints / maxPoints;
            playerHealthText.text = $"{Mathf.RoundToInt(currentPoints)}/{maxPoints}";
        }
    }
}
