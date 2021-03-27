using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour {
    private AgentController agentController;

    float attackRate = 2f;

    bool attackInProgress = false;

    public void Setup(AgentController agentController) {
        this.agentController = agentController;
    }

    public void Attack(Vector3 pos) {
        if (attackInProgress) return;

        attackInProgress = true;

        StartCoroutine(HandleAttackEnd());

        agentController.Movement.Stop();
        DealDamage(pos);
    }

    public void Attack(AgentController agentController) {
        Attack(agentController.transform.position);
    }

    void DealDamage(Vector3 pos) {
        var damageDistance = 5;
        var damageAngle = 90;
        var agentControllers = Utils.FindColliders<AgentController>(transform.position, damageDistance);

        for (int i = 0; i < agentControllers.Count; i++) {
            var agentController = agentControllers[i];
            var dir = (agentController.transform.position - pos).normalized;
            var angle = Vector3.Angle(dir, pos);

            if (angle <= damageAngle) {
                agentController.Health.TakeDamage(5);
            }
        }
    }

    IEnumerator HandleAttackEnd() {
        yield return new WaitForSeconds(1 / attackRate);

        attackInProgress = false;
    }
}
