using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonoBehaviors
{
    public class PlayerController : MonoBehaviour
    {
        private GameplayManager gameplayManager;
        private GameplayManagerGameUI gameplayManagerGameUI;

        private PlayerControllerMouseHover mouseHover;
        public PlayerControllerMouseHover MouseHover => mouseHover;

        public event System.Action waypointModeOn;
        public event System.Action waypointModeOff;

        public void Setup(GameplayManager gameplayManager, GameplayManagerGameUI gameplayManagerGameUI)
        {
            this.gameplayManager = gameplayManager;
            this.gameplayManagerGameUI = gameplayManagerGameUI;
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

            //
            // START HELPERS
            //
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
            else if (Input.GetKeyUp(KeyCode.H))
            {
                agentCtrl.Agent.health.Heal();
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                waypointModeOn?.Invoke();
            }
            else if (Input.GetKeyUp(KeyCode.T))
            {
                waypointModeOff?.Invoke();
            }
            //
            // END HELPERS
            //
            else if (Input.GetKeyUp(KeyCode.U))
            {
                gameplayManager.GameUI.ToggleQuestsPanel();
            }
            else if (Input.GetKeyUp(KeyCode.I))
            {
                gameplayManager.GameUI.ToggleEquipmentPanel();
            }
            else if (Input.GetKeyUp(KeyCode.M))
            {
                if (agentCtrl.Agent.movement.CurrentMovementMode == GameCore.AgentMovement.MovementMode.Walking)
                {
                    agentCtrl.Agent.movement.SetRunningMode();
                }
                else
                {
                    agentCtrl.Agent.movement.SetWalkingMode();
                }
            }
            else if (Input.GetKeyUp(KeyCode.Q))
            {
                agentCtrl.SetActiveSkill(agentCtrl.skills[2]);
                agentCtrl.Attack(mouseHover.GroundHitPoint.With(y: 0));
            }
            else if (Input.GetKeyUp(KeyCode.W))
            {
                agentCtrl.SetActiveSkill(agentCtrl.skills[3]);
                agentCtrl.Attack(mouseHover.GroundHitPoint.With(y: 0));
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetMouseButton(0))
                {
                    agentCtrl.SetActiveSkill(agentCtrl.skills[0]);
                    agentCtrl.Attack(mouseHover.GroundHitPoint.With(y: 0));
                }
                //
                // HELPERS START
                //
                if (Input.GetMouseButtonDown(1))
                {
                    StartCoroutine(
                        agentCtrl.ForceUpdatePosition(mouseHover.GroundHitPoint, gameplayManagerGameUI.CameraFollow)
                    );
                }
                //
                // HELPERS END
                //
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
                    agentCtrl.SetDestination(new Vector3(mouseHover.GroundHitPoint.x, 0, mouseHover.GroundHitPoint.z));
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
