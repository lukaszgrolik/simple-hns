using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonoBehaviors
{
    public class PlayerController : MonoBehaviour
    {
        private GameplayManager gameplayManager;

        private bool groundHitFound;
        private Vector3 groundHitPoint;

        public void Setup(GameplayManager gameplayManager)
        {
            this.gameplayManager = gameplayManager;
        }

        void Update()
        {
            if (gameplayManager == null) return;
            if (gameplayManager.Cam == null) return;
            if (gameplayManager.ControlledAgent == null) return;

            var ray = gameplayManager.Cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo, 100, gameplayManager.GroundLayerMask))
            {
                groundHitFound = true;
                groundHitPoint = hitInfo.point;
            }
            else
            {
                groundHitFound = false;
            }

            var agentCtrl = gameplayManager.ControlledAgent;

            if (Input.GetKeyUp(KeyCode.F1))
            {
                SceneManager.LoadScene(0);
            }

            if (Input.GetKeyUp(KeyCode.A))
            {
                gameplayManager.SaveData.level += 1;
                gameplayManager.Save();
            }

            if (Input.GetKeyUp(KeyCode.S))
            {
                gameplayManager.SaveData.health -= 15;
                gameplayManager.Save();
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                Debug.Log($"health: {gameplayManager.SaveData.health}, level: {gameplayManager.SaveData.level}");
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetMouseButton(0))
                {
                    agentCtrl.SetActiveSkill(agentCtrl.skills[0]);
                    agentCtrl.Attack(groundHitPoint.With(y: 0));
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (groundHitFound)
                {
                    // agent.Movement.SetDestination(groundHitPoint.With(y: 0));
                    agentCtrl.SetDestination(new Vector2(groundHitPoint.x, groundHitPoint.z));
                }
            }
            else if (Input.GetMouseButton(1))
            {
                agentCtrl.SetActiveSkill(agentCtrl.skills[1]);
                agentCtrl.Attack(groundHitPoint.With(y: 0));
            }
        }
    }
}
