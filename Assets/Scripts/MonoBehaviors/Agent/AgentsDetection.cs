using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonoBehaviors
{
    public class AgentsDetection : MonoBehaviour
    {
        private GameplayManager gameplayManager;
        private GameCore.Agent agent;
        //     private AgentController agentController;

        [SerializeField] private SphereCollider sphereCollider;

        // public void Setup(AgentController agentController)
        public void Setup(
            GameplayManager gameplayManager,
            GameCore.Agent agent
        )
        {
            this.gameplayManager = gameplayManager;
            this.agent = agent;
            // this.agentController = agentController;

            // agentController.Health.died.AddListener(OnAgentDied);

            sphereCollider.radius = agent.agentDetection.SightRadius;
        }

        void OnTriggerEnter(Collider coll)
        {
            var isAgent = gameplayManager.dict_object_agentCtrl.TryGetValue(coll.gameObject, out var otherAgent);
            // Debug.Log($"isAgent: {isAgent}");

            if (isAgent)
            {
                agent.agentDetection.AddDetectedAgent(otherAgent.Agent);
            }
        }

        void OnTriggerExit(Collider coll)
        {
            var isAgent = gameplayManager.dict_object_agentCtrl.TryGetValue(coll.gameObject, out var otherAgent);
            // Debug.Log($"isAgent: {isAgent}");

            if (isAgent)
            {
                agent.agentDetection.RemoveDetectedAgent(otherAgent.Agent);
            }
        }

        //     void OnAgentDied(AgentController agent)
        //     {
        //         var colls = GetComponents<Collider>();

        //         for (int i = 0; i < colls.Length; i++)
        //         {
        //             colls[i].enabled = false;
        //         }
        //     }
    }
}