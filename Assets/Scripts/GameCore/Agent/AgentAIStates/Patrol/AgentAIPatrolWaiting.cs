using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.AgentAI.States.PatrolStates
{
    class Waiting : SM.State, IAgentAITickableState
    {
        private Agent agent;
        private EngineTime engineTime;

        private float waitingTimeMin = 1f;
        private float waitingTimeMax = 3f;

        private float waitingTime;
        private float startTime;

        public Waiting(SM.StateMachine stateMachine, Agent agent) : base(stateMachine)
        {
            this.agent = agent;
            this.engineTime = agent.game.engineTime;
        }

        public override void Enter()
        {
            waitingTime = Random.Range(waitingTimeMin, waitingTimeMax);
            startTime = engineTime.Time;
        }

        public override void Exit()
        {

        }

        public void OnUpdate()
        {
            if (engineTime.SecondsFrom(startTime) >= waitingTime)
            {
                stateMachine.SetState(new PatrolStates.Moving(stateMachine, agent));
            }
        }
    }
}