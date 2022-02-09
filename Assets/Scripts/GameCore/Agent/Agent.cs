using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore
{
    public class Agent
    {
        public readonly AgentHealth health;
        public readonly AgentMovement movement;
        public readonly AgentCombat combat;

        public readonly DataDefinition.Agent agentData;

        public Agent(
            AgentHealth health,
            AgentMovement movement,
            AgentCombat combat,
            DataDefinition.Agent agentData
        )
        {
            this.health = health;
            this.movement = movement;
            this.combat = combat;
            this.agentData = agentData;
        }

        public void OnUpdate(float deltaTime)
        {
            combat.OnUpdate(deltaTime);
        }
    }
}