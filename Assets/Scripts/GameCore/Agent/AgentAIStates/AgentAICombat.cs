using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Combat states:
        SelectAction (target & skill (Engage), Rest, ChangePosition, Flee) - to deal damage most effectively
        Engage states:
            AdjustPosition - get close for melee attack; get close enough (or a little further) for range attack
            UseSkill
*/

namespace GameCore.AgentAI.States
{
    // choose action (target & attack OR some (defensive) spell)
    // choose target | select melee/range skill | get close (or get further if to close in case of range attack)
    // attack | wait for attack end | attack again

    namespace CombatStates.EngageStates
    {
        class AdjustPosition : SM.State
        {
            private readonly Agent agent;
            private readonly Agent enemyAgent;

            public AdjustPosition(Agent agent, Agent enemyAgent)
            {
                this.agent = agent;
                this.enemyAgent = enemyAgent;
            }

            public override void Enter()
            {
                // agent.movement.arrived += OnAgentArrived;

                // adjust position ...
                // @todo handle when straight line from enemy agent is obstructed (selected pos is blocked)

                var agentPos = agent.GetPosition();
                var enemyAgentPos = enemyAgent.GetPosition();
                var dir = (agentPos - enemyAgentPos).normalized;
                var targetPos = Vector3.zero;

                if (agent.combat.ActiveSkill is MeleeAttackSkill)
                {
                    // get under 1.5f

                    targetPos = enemyAgentPos + dir * 1f;
                }
                else if (agent.combat.ActiveSkill is ProjectileSkill)
                {
                    // get between 10f and 4f

                    targetPos = enemyAgentPos + dir * 7f;
                }
                else
                {
                    throw new System.Exception($"unhandled skill type: ${agent.combat.ActiveSkill}");
                }

                agent.movement.SetDestination(targetPos);
            }

            public override void Exit()
            {
                // agent.movement.arrived -= OnAgentArrived;
            }

            void OnAgentArrived()
            {
                // stateMachine.Restart();
            }
        }

        class UseSkill : SM.State
        {
            private readonly Agent agent;
            private readonly Agent enemyAgent;

            public UseSkill(Agent agent, Agent enemyAgent)
            {
                this.agent = agent;
                this.enemyAgent = enemyAgent;
            }

            public override void Enter()
            {
                // agent.combat.attackFinished += OnAttackFinished;
                // @todo handle attack broken

                agent.combat.Attack(enemyAgent);
            }

            public override void Exit()
            {
                // agent.combat.attackFinished -= OnAttackFinished;
            }

            void OnAttackFinished()
            {
                // stateMachine.Restart();
            }
        }
    }

    namespace CombatStates
    {
        class Engage : SM.State
        {
            private readonly Agent agent;
            private readonly Agent enemyAgent;

            private SM.StateMachine engageSM = new SM.StateMachine();

            public Engage(Agent agent, Agent enemyAgent)
            {
                this.agent = agent;
                // this.engineTime = agent.game.engineTime;
                this.enemyAgent = enemyAgent;
            }

            public override void Enter()
            {
                agent.stun.stunEnded += OnAgentStunEnded;
                agent.movement.arrived += OnAgentArrived;
                agent.combat.attackFinished += OnAttackFinished;

                SelectAction();
            }

            public override void Exit()
            {
                agent.stun.stunEnded -= OnAgentStunEnded;
                agent.movement.arrived -= OnAgentArrived;
                agent.combat.attackFinished -= OnAttackFinished;

                engageSM.Exit();
            }

            void SelectAction()
            {
                var dist = Vector3.Distance(agent.GetPosition(), enemyAgent.GetPosition());

                if (
                    (agent.combat.ActiveSkill is MeleeAttackSkill && dist > 1.5f) ||
                    (agent.combat.ActiveSkill is ProjectileSkill && (dist > 10f || dist < 4f))
                )
                {
                    engageSM.SetState(new EngageStates.AdjustPosition(agent, enemyAgent));
                }
                else
                {
                    engageSM.SetState(new EngageStates.UseSkill(agent, enemyAgent));
                }
            }

            void OnAgentStunEnded()
            {
                SelectAction();
            }

            void OnAgentArrived()
            {
                SelectAction();
            }

            void OnAttackFinished()
            {
                SelectAction();
            }

            void OnAgentTargetOutsideAttackRange()
            {

            }

            void OnAgentTargetInsideAttackRange()
            {

            }

            void OnAgentTargetObstructed()
            {

            }

            void OnAgentTargetClear()
            {

            }
        }

        class Rest : SM.State
        {

        }

        class ChangePosition : SM.State
        {

        }

        class Flee : SM.State
        {

        }
    }

    public interface ITemp_AgentAICombat
    {
        void OnAliveEnemiesChanged(List<Agent> aliveEnemyAgents);
    }

    class BetterCombat : SM.State, ITemp_AgentAICombat
    {
        private readonly Agent agent;
        private readonly EngineTime engineTime;

        private SM.StateMachine combatSM = new SM.StateMachine();

        private Agent enemyAgent;

        // private float lastAttackTime;

        public BetterCombat(Agent agent)
        {
            this.agent = agent;
            this.engineTime = agent.game.engineTime;
            // this.enemy = enemy;
        }

        public override void Enter()
        {
            // lastAttackTime = engineTime.Time;
            SelectAction();
        }

        public override void Exit()
        {
            combatSM.Exit();
        }

        void SelectAction()
        {
            this.enemyAgent = agent.agentDetection.aliveEnemies.Random();

            Skill skill = null;
            if (agent.combat.ProjectileSkills.Count > 0)
            {
                skill = agent.combat.ProjectileSkills.Random();

            }
            else if (agent.combat.MeleeAttackSkills.Count > 0)
            {
                skill = agent.combat.MeleeAttackSkills.Random();
            }

            agent.combat.SetActiveSkill(skill);

            combatSM.SetState(new CombatStates.Engage(agent, enemyAgent));
        }

        public void OnAliveEnemiesChanged(List<Agent> aliveEnemyAgents)
        {
            if (aliveEnemyAgents.Contains(enemyAgent) == false)
            {
                SelectAction();
            }
        }

        void OnAgentHealthChanged()
        {
            // flee if received enough damage (or any ally nearby got killed or received damage - control via "courage" agent property?)
        }

        // public void OnUpdate()
        // {
        //     if (engineTime.SecondsFrom(lastAttackTime) > .5)
        //     {
        //         lastAttackTime = engineTime.Time;

        //         // @temp set to projectile attack
        //         // agent.combat.SetActiveSkill(agent.combat.Skills[1]);

        //         agent.combat.Attack(enemy);
        //     }
        // }
    }

    class Combat : SM.State, IAgentAITickableState, ITemp_AgentAICombat
    {
        private readonly Agent agent;
        private readonly EngineTime engineTime;

        private Agent enemy;

        private float lastAttackTime;

        // public Combat(SM.StateMachine stateMachine, Agent agent, Agent enemy) : base(stateMachine)
        public Combat(Agent agent)
        {
            this.agent = agent;
            this.engineTime = agent.game.engineTime;
            // this.enemy = enemy;
        }

        public override void Enter()
        {
            var enemy = agent.agentDetection.aliveEnemies.Random();
            if (enemy != null)
            {
                this.enemy = enemy;

                lastAttackTime = engineTime.Time;
            }
        }

        public override void Exit()
        {

        }

        public void OnUpdate()
        {
            if (engineTime.SecondsFrom(lastAttackTime) > .5)
            {
                lastAttackTime = engineTime.Time;

                // @temp set to projectile attack
                // agent.combat.SetActiveSkill(agent.combat.Skills[1]);

                agent.combat.Attack(enemy);
            }
        }

        public void OnAliveEnemiesChanged(List<Agent> aliveEnemyAgents)
        {

        }
    }
}