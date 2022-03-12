using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonoBehaviors
{
    public class PlayerController : MonoBehaviour
    {
        private GameplayManager gameplayManager;

        private PlayerControllerMouseHover mouseHover;
        public PlayerControllerMouseHover MouseHover => mouseHover;

        public void Setup(GameplayManager gameplayManager)
        {
            this.gameplayManager = gameplayManager;
            this.mouseHover = new PlayerControllerMouseHover(gameplayManager);
        }

        void Update()
        {
            if (gameplayManager == null) return;
            if (gameplayManager.Cam == null) return;
            if (gameplayManager.ControlledAgent == null) return;

            mouseHover.OnUpdate();

            var agentCtrl = gameplayManager.ControlledAgent;

            // if (Input.GetKeyUp(KeyCode.F1))
            // {
            //     SceneManager.LoadScene(0);
            // }

            // if (Input.GetKeyUp(KeyCode.A))
            // {
            //     gameplayManager.SaveData.level += 1;
            //     gameplayManager.Save();
            // }

            // if (Input.GetKeyUp(KeyCode.S))
            // {
            //     gameplayManager.SaveData.health -= 15;
            //     gameplayManager.Save();
            // }

            // if (Input.GetKeyUp(KeyCode.Space))
            // {
            //     Debug.Log($"health: {gameplayManager.SaveData.health}, level: {gameplayManager.SaveData.level}");
            // }

            if (Input.GetKey(KeyCode.D))
            {
                if (Input.GetKeyUp(KeyCode.Alpha1))
                {
                    PlayerControllerUtils.DropRandom(agentCtrl);
                }
            }
            else if (Input.GetKeyUp(KeyCode.F))
            {
                PlayerControllerUtils.PickClosest(agentCtrl);
            }
            else if (Input.GetKeyUp(KeyCode.U))
            {
                gameplayManager.GameUI.ToggleQuestsPanel();
            }
            else if (Input.GetKeyUp(KeyCode.I))
            {
                gameplayManager.GameUI.ToggleEquipmentPanel();
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetMouseButton(0))
                {
                    agentCtrl.SetActiveSkill(agentCtrl.skills[0]);
                    agentCtrl.Attack(mouseHover.GroundHitPoint.With(y: 0));
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (mouseHover.DroppedItemHitFound)
                {
                    agentCtrl.Agent.equipment.Pick(mouseHover.DroppedItemHitItem);
                }
                else if (mouseHover.GroundHitFound)
                {
                    // agent.Movement.SetDestination(mouseHover.GroundHitPoint.With(y: 0));
                    agentCtrl.SetDestination(new Vector2(mouseHover.GroundHitPoint.x, mouseHover.GroundHitPoint.z));
                }
            }
            else if (Input.GetMouseButton(1))
            {
                agentCtrl.SetActiveSkill(agentCtrl.skills[1]);
                agentCtrl.Attack(mouseHover.GroundHitPoint.With(y: 0));
            }
        }
    }
}
