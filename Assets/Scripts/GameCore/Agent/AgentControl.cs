using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public abstract class AgentControl : AgentComponent
    {
        public AgentControl()
        {

        }

        public abstract void Setup();
    }

    public interface IAgentControlTickable
    {
        void OnUpdate();
    }

    public interface IAgentAITickableState
    {
        void OnUpdate();
    }
}