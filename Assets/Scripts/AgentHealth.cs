using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class AgentHealth : MonoBehaviour {
    private int healthPoints = 100;

    public void Setup() {

    }

    public void TakeDamage(int damagePoints) {
        healthPoints -= damagePoints;
        if (healthPoints < 0) healthPoints = 0;

        if (healthPoints == 0) {
            // die
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos() {
        Handles.Label(transform.position, healthPoints.ToString());
    }
#endif
}
