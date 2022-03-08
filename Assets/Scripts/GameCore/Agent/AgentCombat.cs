using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    // public interface ISkillAgentCombatGame :
    // {

    // }

    public abstract class Skill
    {
        protected AgentCombat combat;

        public Skill(AgentCombat combat)
        {
            this.combat = combat;
        }

        public abstract void Trigger(Vector3 targetPos);
    }

    public sealed class MeleeAttackSkill : Skill
    {
        public MeleeAttackSkill(AgentCombat combat) : base(combat)
        {

        }

        public override void Trigger(Vector3 targetPos)
        {
            DealDamage(targetPos);
        }

        void DealDamage(Vector3 targetPos)
        {
            var game = combat.game;

            var damageDistance = 2;
            var damageAngle = 90;
            // var agentControllers = Utils.FindColliders<AgentController>(combat.transform.position, damageDistance);
            var otherAgents = game.FindAgentsInRadius(game.GetPosition(combat.Agent), damageDistance);

            combat.InvokeMeleeAttackStarted();

            for (int i = 0; i < otherAgents.Count; i++)
            {
                var otherAgent = otherAgents[i];

                var isAliveEnemy = combat.Agent.partyMember.AgentsParty.IsAliveEnemy(otherAgent);
                if (!isAliveEnemy) continue;

                // var dir = (agent.transform.position - targetPos).normalized;
                var dir = (game.GetPosition(otherAgent) - targetPos).normalized;
                var angle = Vector3.Angle(dir, targetPos);

                if (angle <= damageAngle)
                {
                    otherAgent.health.TakeDamage(5);
                }
            }
        }
    }

    public sealed class ProjectileSkill : Skill
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
            // Debug.Log("SpawnProjectile");

            var game = combat.game;

            // var origin = combat.transform;
            var originPos = game.GetPosition(combat.Agent);
            // var dir = (targetPos - originPos).normalized;
            // var angle = Vector3.Angle(dir, origin.forward);
            var angle = 90 - Mathf.Atan2(targetPos.z - originPos.z, targetPos.x - originPos.x) * Mathf.Rad2Deg;
            // Debug.Log(angle);

            // var angle = Vector3.Angle(originPos, targetPos);
            var rot = Quaternion.Euler(0f, angle, 0f);

            var projectile = new Projectile(
                game: game,
                originatorAgent: combat.Agent
            );

            combat.InvokeCastingStarted();

            // game.SpawnProjectile(combat.ProjectileSpawnPoint.position, rot);
            game.SpawnProjectile(projectile, game.GetProjectileSpawnPosition(combat.Agent), rot);
        }
    }

    public class AgentCombat : AgentComponent
    {
        public readonly Game game;
        public readonly AgentMovement movement;

        private readonly List<Skill> skills = new List<Skill>();
        public IReadOnlyList<Skill> Skills => skills;

        private float attackRate = 5f;

        private Skill activeSkill;
        private bool attackInProgress = false;
        private float attackInProgressElapsed = 0;

        public event System.Action meleeAttackStarted;
        public event System.Action castingStarted;

        public AgentCombat(
            Game game,
            AgentMovement movement
        )
        {
            this.game = game;
            this.movement = movement;

            var skills = new List<Skill>(){
                new MeleeAttackSkill(this),
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

        public void InvokeMeleeAttackStarted()
        {
            meleeAttackStarted?.Invoke();
        }

        public void InvokeCastingStarted()
        {
            castingStarted?.Invoke();
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

        public void Attack(Agent agent)
        {
            Attack(game.GetPosition(agent));
        }

        // IEnumerator HandleAttackEnd()
        // {
        //     yield return new WaitForSeconds(1 / attackRate);

        //     attackInProgress = false;
        // }
    }
}