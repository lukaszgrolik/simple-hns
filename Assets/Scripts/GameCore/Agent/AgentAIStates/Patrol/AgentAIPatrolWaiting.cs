using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.AgentAI.States.PatrolStates
{
    class Waiting : SM.State, IAgentAITickableState
    {
        private Agent agent;

        private IEnumerator waitCoroutine;

        public Waiting(SM.StateMachine stateMachine, Agent agent) : base(stateMachine)
        {
            this.agent = agent;
        }

        public override void Enter()
        {
            // waitCoroutine = Wait();
            // agent.StartCoroutine(waitCoroutine);
        }

        public override void Exit()
        {
            // if (waitCoroutine != null) agent.StopCoroutine(waitCoroutine);
        }

        public void OnUpdate()
        {

        }

        // private IEnumerator Wait()
        // {
        //     yield return new WaitForSeconds(1);

        //     stateMachine.SetState(new PatrolStates.Moving(stateMachine, agent));
        // }
    }
}