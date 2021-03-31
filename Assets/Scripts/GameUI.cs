using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour {
    [SerializeField] Image playerHealthImage;
    [SerializeField] TMP_Text playerHealthText;
    // [SerializeField] Image enemyHealthImage;

    public void SetPlayerHealth(int currentPoints, int maxPoints) {
        playerHealthImage.fillAmount = (float)currentPoints / maxPoints;
        playerHealthText.text = $"{currentPoints}/{maxPoints}";
    }
}
