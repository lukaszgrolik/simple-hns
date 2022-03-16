using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.AgentAI.States.PatrolStates
{
    class Moving : SM.State
    {
        private Agent agent;

        private float radiusMin = 3f;
        private float radiusMax = 6f;

        public Moving(SM.StateMachine stateMachine, Agent agent) : base(stateMachine)
        {
            this.agent = agent;
        }

        public override void Enter()
        {
            agent.movement.arrived += OnAgentArrived;
            agent.movement.SetDestination(GetRandomPos());
        }

        public override void Exit()
        {
            agent.movement.arrived -= OnAgentArrived;
        }

        Vector3 GetRandomPos()
        {
            // var currentPos = agent.game.GetPosition(agent);
            var originPos = agent.groupMember.AgentsGroup.GetCenter();
            var circlePos = Random.insideUnitCircle.normalized * Random.Range(radiusMin, radiusMax);
            var endPos = originPos + new Vector3(circlePos.x, 0, circlePos.y);

            return endPos;
        }

        void OnAgentArrived()
        {
            stateMachine.SetState(new PatrolStates.Waiting(stateMachine, agent));
        }
    }
}