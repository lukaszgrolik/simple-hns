using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MonoBehaviors
{
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
}
