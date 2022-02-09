using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore
{
    public abstract class Skill
    {
        protected AgentCombat combat;

        public Skill(AgentCombat combat)
        {
            this.combat = combat;
        }

        public abstract void Trigger(Vector3 targetPos);
    }

    // public class MeleeAttackSkill : Skill
    // {
    //     public MeleeAttackSkill(AgentCombat combat) : base(combat)
    //     {

    //     }

    //     public override void Trigger(Vector3 targetPos)
    //     {
    //         DealDamage(targetPos);
    //     }

    //     void DealDamage(Vector3 targetPos)
    //     {
    //         var damageDistance = 5;
    //         var damageAngle = 90;
    //         var agentControllers = Utils.FindColliders<AgentController>(combat.transform.position, damageDistance);

    //         for (int i = 0; i < agentControllers.Count; i++)
    //         {
    //             var agentController = agentControllers[i];
    //             var dir = (agentController.transform.position - targetPos).normalized;
    //             var angle = Vector3.Angle(dir, targetPos);

    //             if (angle <= damageAngle)
    //             {
    //                 agentController.TakeDamage(5);
    //             }
    //         }
    //     }
    // }

    public class ProjectileSkill : Skill
    {
        public ProjectileSkill(AgentCombat combat) : base(combat)
        {

        }

        public override void Trigger(Vector3 targetPos)
        {
            SpawnProjectile(targetPos);
        }

        void SpawnProjectile(Vector3 targetPos)
        {
            Debug.Log("SpawnProjectile");
            // var gm = combat.AgentController.GameplayManager;

            // var origin = combat.transform;
            // var originPos = origin.position;
            // // var dir = (targetPos - originPos).normalized;
            // // var angle = Vector3.Angle(dir, origin.forward);
            // var angle = 90 - Mathf.Atan2(targetPos.z - originPos.z, targetPos.x - originPos.x) * Mathf.Rad2Deg;
            // // Debug.Log(angle);

            // // var angle = Vector3.Angle(originPos, targetPos);
            // var rot = Quaternion.Euler(0f, angle, 0f);
            // var obj = GameObject.Instantiate(gm.ProjectilePrefab, combat.ProjectileSpawnPoint.position, rot, gm.ProjectilesContainer);
            // var projectile = obj.GetComponent<Projectile>();
            // projectile.Setup(combat.AgentController);
        }
    }

    public class AgentCombat
    {
        private AgentMovement movement;

        private readonly List<Skill> skills = new List<Skill>();
        public IReadOnlyList<Skill> Skills => skills;

        private float attackRate = 5f;

        private Skill activeSkill;
        private bool attackInProgress = false;
        private float attackInProgressElapsed = 0;

        public AgentCombat(
            AgentMovement movement
        )
        {
            this.movement = movement;

            var skills = new List<Skill>(){
                // new MeleeAttackSkill(this),
                new ProjectileSkill(this),
            };
            this.skills.AddRange(skills);

            activeSkill = skills[0];
        }

        public void OnUpdate(float deltaTime)
        {
            if (attackInProgress)
            {
                attackInProgressElapsed += deltaTime;
                if (attackInProgressElapsed >= 1 / attackRate)
                {
                    attackInProgressElapsed = 0;
                    attackInProgress = false;
                }
            }
        }

        public void SetActiveSkill(Skill skill)
        {
            if (skills.Contains(skill) == false) return;

            activeSkill = skill;
        }

        public void Attack(Vector3 pos)
        {
            if (attackInProgress) return;

            attackInProgress = true;

            // StartCoroutine(HandleAttackEnd());

            movement.Cancel();

            activeSkill.Trigger(pos);
        }

        // public void Attack(AgentController agentController)
        // {
        //     Attack(agentController.transform.position);
        // }

        // IEnumerator HandleAttackEnd()
        // {
        //     yield return new WaitForSeconds(1 / attackRate);

        //     attackInProgress = false;
        // }
    }
}