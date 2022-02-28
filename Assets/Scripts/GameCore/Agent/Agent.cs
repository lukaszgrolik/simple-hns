using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AgentComponent
    {
        protected Agent agent;
        public Agent Agent => agent;

        public void SetAgent(Agent agent)
        {
            if (this.agent != null) throw new System.Exception("agent already set");
            this.agent = agent;
        }
    }

    public class Agent : ITransformScript
    {
        public readonly Game game;

        public readonly AgentHealth health;
        public readonly AgentMovement movement;
        public readonly AgentPartyMember partyMember;
        public readonly AgentDetection agentDetection;
        public readonly AgentCombat combat;
        public readonly AgentControl control;

        public readonly DataDefinition.Agent agentData;

        public Agent(
            Game game,
            AgentHealth health,
            AgentMovement movement,
            AgentPartyMember partyMember,
            AgentDetection agentDetection,
            AgentCombat combat,
            AgentControl control,
            DataDefinition.Agent agentData
        )
        {
            this.game = game;

            this.health = health;
            this.movement = movement;

            this.partyMember = partyMember;
            partyMember.SetAgent(this);

            this.agentDetection = agentDetection;
            agentDetection.SetAgent(this);

            this.combat = combat;
            combat.SetAgent(this);

            this.control = control;
            control.SetAgent(this);

            this.agentData = agentData;
        }

        public void Setup()
        {
            control.Setup();
        }

        public void OnUpdate(float deltaTime)
        {
            combat.OnUpdate(deltaTime);
        }
    }
}