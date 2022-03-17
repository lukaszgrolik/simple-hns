using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public sealed class AgentNpcAIControl : AgentControl, IAgentControlTickable
    {
        // WalkingAround, AttendingToPlayer
        private SM.StateMachine sm = new SM.StateMachine();

        public override void Setup()
        {

        }

        public void OnUpdate()
        {

        }
    }
}