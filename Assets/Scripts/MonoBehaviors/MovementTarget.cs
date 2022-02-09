using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonoBehaviors
{
    public class MovementTarget : MonoBehaviour
    {
        private AgentController agent;

        public void Setup(AgentController agent)
        {
            this.agent = agent;
        }

        void OnTriggerEnter(Collider info)
        {
            if (agent.GameplayManager.AgentObjectsControllers.TryGetValue(info.gameObject, out var otherAgent))
            {
                if (agent == otherAgent)
                {
                    agent.MarkArrived();

                    Destroy(gameObject);
                }
            }
        }
    }
}
