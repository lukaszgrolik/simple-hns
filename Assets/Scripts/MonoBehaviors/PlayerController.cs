using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonoBehaviors
{
    public class PlayerControllerMouseHover
    {
        private GameplayManager gameplayManager;

        private bool groundHitFound;
        public bool GroundHitFound => groundHitFound;

        private Vector3 groundHitPoint;
        public Vector3 GroundHitPoint => groundHitPoint;

        private bool agentHitFound;
        private GameCore.Agent agentHitAgent;

        public event System.Action<GameCore.Agent> agentMouseEntered;
        public event System.Action<GameCore.Agent> agentMouseLeft;

        public PlayerControllerMouseHover(GameplayManager gameplayManager)
        {
            this.gameplayManager = gameplayManager;
        }

        public void OnUpdate()
        {
            var ray = gameplayManager.Cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var groundHitInfo, 100, gameplayManager.GroundLayerMask))
            {
                groundHitFound = true;
                groundHitPoint = groundHitInfo.point;
            }
            else
            {
                groundHitFound = false;
            }

            if (Physics.Raycast(ray, out var agentHitInfo, 100, gameplayManager.AgentLayerMask))
            {
                if (agentHitFound == false)
                {
                    agentHitFound = true;
                    agentHitAgent = gameplayManager.Dict_object_agentCtrl[agentHitInfo.collider.gameObject].Agent;

                    agentMouseEntered?.Invoke(agentHitAgent);
                }
            }
            else
            {
                if (agentHitFound)
                {
                    agentMouseLeft?.Invoke(agentHitAgent);
                }

                agentHitFound = false;
            }
        }
    }

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

            if (Input.GetKey(KeyCode.D))
            {
                if (Input.GetKeyUp(KeyCode.Alpha1))
                {
                    var itemDataList = new List<DataDefinition.Item>(){
                        DataInstance.Items.handAxe,
                        DataInstance.Items.shortStaff,
                        DataInstance.Items.shortSword,
                        DataInstance.Items.skullCap,
                        DataInstance.Items.smallShield,
                    };
                    var itemData = itemDataList.Random();
                    var item = new GameCore.Item(
                        itemData: itemData
                    );

                    var circlePos = Random.insideUnitCircle * 3;
                    var pos = agentCtrl.transform.position + new Vector3(circlePos.x, 0, circlePos.y);

                    gameplayManager.Game.itemSystem.Drop(item, pos);
                }
            }

            if (Input.GetKeyUp(KeyCode.F))
            {
                var droppedItems = Utils.FindColliders<DroppedItemMB>(agentCtrl.transform.position, 2, gameplayManager.DroppedItemLayerMask);
                if (droppedItems.Count > 0)
                {
                    var droppedItems_closest = Utils.SortByProximity(droppedItems, agentCtrl.transform.position);

                    agentCtrl.Agent.equipment.Pick(droppedItems_closest[0].DroppedItem);
                }
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetMouseButton(0))
                {
                    agentCtrl.SetActiveSkill(agentCtrl.skills[0]);
                    agentCtrl.Attack(mouseHover.GroundHitPoint.With(y: 0));
                }
            }
            else if (Input.GetMouseButton(0))
            {
                if (mouseHover.GroundHitFound)
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
