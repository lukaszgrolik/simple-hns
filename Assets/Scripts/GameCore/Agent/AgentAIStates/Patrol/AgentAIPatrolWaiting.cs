using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.AgentAI.States.PatrolStates
{
    class Waiting : SM.State, IAgentAITickableState
    {
        private Agent agent;
        private EngineTime engineTime;

        private float startTime;

        public Waiting(SM.StateMachine stateMachine, Agent agent) : base(stateMachine)
        {
            this.agent = agent;
            this.engineTime = agent.game.engineTime;
        }

        public override void Enter()
        {
            startTime = engineTime.Time;
        }

        public override void Exit()
        {

        }

        public void OnUpdate()
        {
            if (engineTime.SecondsFrom(startTime) >= 1)
            {
                stateMachine.SetState(new PatrolStates.Moving(stateMachine, agent));
            }
        }
    }
}