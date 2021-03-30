using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementTarget : MonoBehaviour {
    private AgentController agent;

    public void Setup(AgentController agent) {
        this.agent = agent;
    }

    void OnTriggerEnter(Collider info) {
        if (agent.GameplayManager.AgentsManager.Agents.TryGetValue(info.gameObject, out var otherAgent)) {
            if (agent == otherAgent) {
                agent.Movement.Arrived.Invoke();

                Destroy(gameObject);
            }
        }
    }
}
