using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour {
    private AgentMovement agentMovement;
    public AgentMovement Movement => agentMovement;

    private AgentHealth agentHealth;
    public AgentHealth Health => agentHealth;

    private Combat combat;
    public Combat Combat => combat;

    public void Setup(AgentsManager.Party party) {
        agentMovement = GetComponent<AgentMovement>();
        agentMovement.Setup();

        agentHealth = GetComponent<AgentHealth>();
        agentHealth.Setup();

        combat = GetComponent<Combat>();
        combat.Setup(this);

        var patrol = GetComponent<Patrol>();
        if (patrol) patrol.Setup(this);
    }
}
