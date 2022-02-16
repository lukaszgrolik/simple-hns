using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore
{
    public class Agent : ITransformScript
    {
        public readonly AgentHealth health;
        public readonly AgentMovement movement;
        public readonly AgentPartyMember partyMember;
        public readonly AgentCombat combat;

        public readonly DataDefinition.Agent agentData;

        public Agent(
            AgentHealth health,
            AgentMovement movement,
            AgentPartyMember partyMember,
            AgentCombat combat,
            DataDefinition.Agent agentData
        )
        {
            this.health = health;
            this.movement = movement;
            this.partyMember = partyMember;
            this.combat = combat; combat.SetAgent(this);
            this.agentData = agentData;
        }

        public void OnUpdate(float deltaTime)
        {
            combat.OnUpdate(deltaTime);
        }
    }
}