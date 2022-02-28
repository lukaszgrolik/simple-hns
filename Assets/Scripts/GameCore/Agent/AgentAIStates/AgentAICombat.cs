using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.AgentAI.States
{
    class Combat : SM.State, IAgentAITickableState
    {
        private Agent agent;
        private Agent enemy;

        // IEnumerator keepAttackingCoroutine;

        public Combat(SM.StateMachine stateMachine, Agent agent, Agent enemy) : base(stateMachine)
        {
            this.agent = agent;
            this.enemy = enemy;
        }

        public override void Enter()
        {
            // keepAttackingCoroutine = KeepAttacking();
            // agent.StartCoroutine(keepAttackingCoroutine);
        }

        public override void Exit()
        {
            // if (keepAttackingCoroutine != null) agent.StopCoroutine(keepAttackingCoroutine);
        }

        public void OnUpdate()
        {

        }

        // IEnumerator KeepAttacking()
        // {
        //     while (true)
        //     {
        //         yield return new WaitForSeconds(.5f);

        //         agent.combat.Attack(enemy);
        //     }
        // }
    }
}