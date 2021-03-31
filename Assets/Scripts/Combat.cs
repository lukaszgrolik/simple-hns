using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill {
    protected Combat combat;

    public Skill(Combat combat) {
        this.combat = combat;
    }

    public abstract void Trigger(Vector3 targetPos);
}

public class MeleeAttackSkill : Skill {
    public MeleeAttackSkill(Combat combat) : base(combat) {

    }

    public override void Trigger(Vector3 targetPos) {
        DealDamage(targetPos);
    }

    void DealDamage(Vector3 targetPos) {
        var damageDistance = 5;
        var damageAngle = 90;
        var agentControllers = Utils.FindColliders<AgentController>(combat.transform.position, damageDistance);

        for (int i = 0; i < agentControllers.Count; i++) {
            var agentController = agentControllers[i];
            var dir = (agentController.transform.position - targetPos).normalized;
            var angle = Vector3.Angle(dir, targetPos);

            if (angle <= damageAngle) {
                agentController.Health.TakeDamage(5);
            }
        }
    }
}

public class ProjectileSkill : Skill {
    public ProjectileSkill(Combat combat) : base(combat) {

    }

    public override void Trigger(Vector3 targetPos) {
        SpawnProjectile(targetPos);
    }

    void SpawnProjectile(Vector3 targetPos) {
        var gm = combat.AgentController.GameplayManager;

        var origin = combat.transform;
        var originPos = origin.position;
        // var dir = (targetPos - originPos).normalized;
        // var angle = Vector3.Angle(dir, origin.forward);
        var angle = 90 - Mathf.Atan2(targetPos.z - originPos.z, targetPos.x - originPos.x) * Mathf.Rad2Deg;
        // Debug.Log(angle);

        // var angle = Vector3.Angle(originPos, targetPos);
        var rot = Quaternion.Euler(0f, angle, 0f);
        var obj = GameObject.Instantiate(gm.ProjectilePrefab, combat.ProjectileSpawnPoint.position, rot, gm.ProjectilesContainer);
        var projectile = obj.GetComponent<Projectile>();
        projectile.Setup(combat.AgentController);
    }
}

public class Combat : MonoBehaviour {
    AgentController agentController;
    public AgentController AgentController => agentController;

    [SerializeField] Transform projectileSpawnPoint;
    public Transform ProjectileSpawnPoint => projectileSpawnPoint;

    readonly List<Skill> skills = new List<Skill>();
    public List<Skill> Skills => skills;

    float attackRate = 5f;

    Skill activeSkill;
    bool attackInProgress = false;

    public void Setup(AgentController agentController) {
        this.agentController = agentController;

        skills.AddRange(new List<Skill>(){
            new MeleeAttackSkill(this),
            new ProjectileSkill(this),
        });

        activeSkill = skills[0];
    }

    public void SetActiveSkill(Skill skill) {
        if (skills.Contains(skill) == false) return;

        activeSkill = skill;
    }

    public void Attack(Vector3 pos) {
        if (attackInProgress) return;

        attackInProgress = true;

        StartCoroutine(HandleAttackEnd());

        agentController.Movement.Stop();

        activeSkill.Trigger(pos);
    }

    public void Attack(AgentController agentController) {
        Attack(agentController.transform.position);
    }

    IEnumerator HandleAttackEnd() {
        yield return new WaitForSeconds(1 / attackRate);

        attackInProgress = false;
    }
}
