using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AgentHealth : MonoBehaviour {
    private AgentController agentController;

    private int healthPoints = 100;

    public class DiedEvent : UnityEvent<AgentController> {}
    private DiedEvent died = new DiedEvent();
    public DiedEvent Died => died;

    public void Setup(AgentController agentController) {
        this.agentController = agentController;
    }

    public void TakeDamage(int damagePoints) {
        healthPoints -= damagePoints;
        if (healthPoints < 0) healthPoints = 0;

        if (healthPoints == 0) {
            Die();
        }
    }

    void Die() {
        died.Invoke(agentController);
    }

#if UNITY_EDITOR
    void OnDrawGizmos() {
        Handles.Label(transform.position, healthPoints.ToString());
    }
#endif
}
