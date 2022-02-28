using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.AgentAI.States
{
    class Patrol : SM.State, IAgentAITickableState
    {
        private SM.StateMachine patrolSM = new SM.StateMachine();

        private Agent agent;

        public Patrol(SM.StateMachine stateMachine, Agent agent) : base(stateMachine)
        {
            this.agent = agent;
        }

        public override void Enter()
        {
            patrolSM.SetState(new PatrolStates.Moving(stateMachine, agent));
        }

        public override void Exit()
        {
            patrolSM.Exit();
        }

        public void OnUpdate()
        {
            if (patrolSM.State is IAgentAITickableState state)
            {
                state.OnUpdate();
            }
        }
    }
}