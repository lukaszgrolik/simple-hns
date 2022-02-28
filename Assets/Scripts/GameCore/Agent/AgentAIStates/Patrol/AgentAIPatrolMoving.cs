using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.AgentAI.States.PatrolStates
{
    class Moving : SM.State
    {
        private Agent agent;

        private float radius = 5f;

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
            var randomPos = Random.insideUnitCircle * radius;

            // return agent.transform.position + new Vector3(randomPos.x, 0, randomPos.y);
            // Debug.Log($"game: {agent.game}");
            var pos = agent.game.GetPosition(agent);
            // Debug.Log($"pos: {pos}");

            return pos + new Vector3(randomPos.x, 0, randomPos.y);
        }

        void OnAgentArrived()
        {
            stateMachine.SetState(new PatrolStates.Waiting(stateMachine, agent));
        }
    }
}