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
        public MeleeAttackSkill(AgentCombat combat, DataDefinition.Skill_Melee skillData) : base(combat)
        {

        }

        public override void Trigger(Vector3 targetPos)
        {
            DealDamage(targetPos);
        }

        void DealDamage(Vector3 targetPos)
        {
            var game = combat.game;

            var damageDistance = 1.5f;
            var damageAngle = 90;
            // var agentControllers = Utils.FindColliders<AgentController>(combat.transform.position, damageDistance);
            var otherAgents = game.FindAgentsInRadius(game.GetPosition(combat.Agent), damageDistance);
            // Debug.Log($"otherAgents.Count: {otherAgents.Count}");

            combat.InvokeMeleeAttackStarted();

            for (int i = 0; i < otherAgents.Count; i++)
            {
                var otherAgent = otherAgents[i];
                // Debug.Log($"otherAgent.agentData.name: {otherAgent.agentData.name}");

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

    public sealed class CustomSkill : Skill
    {
        public CustomSkill(AgentCombat combat, DataDefinition.Skill_Custom skillData) : base(combat)
        {

        }

        public override void Trigger(Vector3 targetPos)
        {
            Debug.Log("custom skill unimplemented");
        }
    }

    public sealed class BowSkill : Skill
    {
        public BowSkill(AgentCombat combat, DataDefinition.Skill_Bow skillData) : base(combat)
        {

        }

        public override void Trigger(Vector3 targetPos)
        {
            Debug.Log("bow skill unimplemented");
        }
    }

    public sealed class ProjectileSkill : Skill
    {
        private DataDefinition.Skill_CastProjectile skillData;

        public ProjectileSkill(AgentCombat combat, DataDefinition.Skill_CastProjectile skillData) : base(combat)
        {
            this.skillData = skillData;
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
                originatorAgent: combat.Agent,
                projectileSkillData: skillData
            );

            combat.InvokeCastingStarted();

            // game.SpawnProjectile(combat.ProjectileSpawnPoint.position, rot);
            game.SpawnProjectile(projectile, game.GetProjectileSpawnPosition(combat.Agent), rot);
        }
    }

    public sealed class SummonSkill : Skill
    {
        public SummonSkill(AgentCombat combat, DataDefinition.Skill_SummonAgent skillData) : base(combat)
        {

        }

        public override void Trigger(Vector3 targetPos)
        {
            Debug.Log("summon skill unimplemented");
        }
    }

    public class AgentCombat : AgentComponent
    {
        public readonly Game game;
        public readonly AgentMovement movement;

        private List<Skill> skills = new List<Skill>();
        public IReadOnlyList<Skill> Skills => skills;

        private List<Skill> meleeAttackSkills = new List<Skill>();
        public IReadOnlyList<Skill> MeleeAttackSkills => meleeAttackSkills;

        private List<Skill> projectileSkills = new List<Skill>();
        public IReadOnlyList<Skill> ProjectileSkills => projectileSkills;

        private float attackRate = 5f;

        private Skill activeSkill;
        public Skill ActiveSkill => activeSkill;

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

            // var skills = new List<Skill>(){
            //     new MeleeAttackSkill(this),
            //     new ProjectileSkill(this),
            // };
        }

        public void SetSkills(List<Skill> skills)
        {
            if (this.skills.Count > 0) throw new System.Exception("skills already set");

            this.skills.AddRange(skills);

            activeSkill = skills[0];

            for (int i = 0; i < skills.Count; i++)
            {
                if (skills[i] is MeleeAttackSkill)
                {
                    meleeAttackSkills.Add(skills[i]);
                }
                else if (skills[i] is ProjectileSkill)
                {
                    projectileSkills.Add(skills[i]);
                }
            }
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

                    attackFinished?.Invoke();
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
            attackStarted?.Invoke(activeSkill);

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