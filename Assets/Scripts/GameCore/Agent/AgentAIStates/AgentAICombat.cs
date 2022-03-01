using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.AgentAI.States
{
    class Combat : SM.State, IAgentAITickableState
    {
        private Agent agent;
        private EngineTime engineTime;
        private Agent enemy;

        private float lastAttackTime;

        public Combat(SM.StateMachine stateMachine, Agent agent, Agent enemy) : base(stateMachine)
        {
            this.agent = agent;
            this.engineTime = agent.game.engineTime;
            this.enemy = enemy;
        }

        public override void Enter()
        {
            lastAttackTime = engineTime.Time;
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
    }
}