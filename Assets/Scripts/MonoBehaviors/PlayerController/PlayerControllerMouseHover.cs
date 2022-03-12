using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonoBehaviors
{
    public class PlayerControllerMouseHover
    {
        private GameplayManager gameplayManager;

        // --- ground

        private bool groundHitFound;
        public bool GroundHitFound => groundHitFound;

        private Vector3 groundHitPoint;
        public Vector3 GroundHitPoint => groundHitPoint;

        // --- agent

        private bool agentHitFound;
        private GameCore.Agent agentHitAgent;

        // --- dropped item

        private bool droppedItemHitFound;
        public bool DroppedItemHitFound => droppedItemHitFound;

        private GameCore.DroppedItem droppedItemHitItem;
        public GameCore.DroppedItem DroppedItemHitItem => droppedItemHitItem;

        // ---

        public event System.Action<GameCore.Agent> agentMouseEntered;
        public event System.Action<GameCore.Agent> agentMouseLeft;

        public PlayerControllerMouseHover(GameplayManager gameplayManager)
        {
            this.gameplayManager = gameplayManager;
        }

        // @todo @perf OnFixedUpdate?
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
                    agentHitAgent = gameplayManager.dict_object_agentCtrl[agentHitInfo.collider.gameObject].Agent;

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

            if (Physics.Raycast(ray, out var droppedItemHitInfo, 100, gameplayManager.DroppedItemLayerMask))
            {
                droppedItemHitFound = true;
                droppedItemHitItem = gameplayManager.dict_object_droppedItemMB[droppedItemHitInfo.collider.gameObject].DroppedItem;
            }
            else
            {
                droppedItemHitFound = false;
            }
        }
    }
}
