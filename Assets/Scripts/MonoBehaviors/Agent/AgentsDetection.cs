using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonoBehaviors
{
    // public class AgentsDetection : MonoBehaviour
    // {
    //     private AgentController agentController;

    //     public void Setup(AgentController agentController)
    //     {
    //         this.agentController = agentController;

    //         agentController.Health.died.AddListener(OnAgentDied);
    //     }

    //     void OnTriggerEnter(Collider info)
    //     {
    //         if (agentController.AliveEnemies.TryGetValue(info.gameObject, out var otherAgent))
    //         {
    //             // Debug.Log($"entered: {otherAgent}");
    //             if (agentController.VisibleEnemies.Contains(otherAgent) == false)
    //             {
    //                 agentController.AddVisibleEnemy(otherAgent);
    //             }
    //         }
    //     }

    //     void OnTriggerExit(Collider info)
    //     {
    //         if (agentController.AliveEnemies.TryGetValue(info.gameObject, out var otherAgent))
    //         {
    //             // Debug.Log($"exited: {otherAgent}");

    //             agentController.RemoveVisibleEnemy(otherAgent);
    //         }
    //     }

    //     void OnAgentDied(AgentController agent)
    //     {
    //         var colls = GetComponents<Collider>();

    //         for (int i = 0; i < colls.Length; i++)
    //         {
    //             colls[i].enabled = false;
    //         }
    //     }
    // }
}