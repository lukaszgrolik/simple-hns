using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AgentHealth : MonoBehaviour {
    AgentController agentController;

    int maxPoints = 100;
    public int MaxPoints => maxPoints;

    int currentPoints = 100;
    public int CurrentPoints => currentPoints;

    public class HealthChangedEvent : UnityEvent<int, int> {}
    HealthChangedEvent healthChanged = new HealthChangedEvent();
    public HealthChangedEvent HealthChanged => healthChanged;

    public class DiedEvent : UnityEvent<AgentController> {}
    DiedEvent died = new DiedEvent();
    public DiedEvent Died => died;

    public void Setup(AgentController agentController) {
        this.agentController = agentController;
    }

    public void TakeDamage(int damagePoints) {
        currentPoints -= damagePoints;
        if (currentPoints < 0) currentPoints = 0;

        healthChanged.Invoke(currentPoints, maxPoints);

        if (currentPoints == 0) {
            Die();
        }
    }

    void Die() {
        died.Invoke(agentController);
    }

#if UNITY_EDITOR
    void OnDrawGizmos() {
        Handles.Label(transform.position, currentPoints.ToString());
    }
#endif
}
