using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore
{
    public class AgentsParty
    {
        public readonly Game game;
        private DataDefinition.AgentParty agentPartyData; public DataDefinition.AgentParty AgentPartyData => agentPartyData;

        private readonly List<Agent> enemies = new List<Agent>();
        private readonly List<Agent> aliveEnemies = new List<Agent>();

        public AgentsParty(
            Game game,
            DataDefinition.AgentParty agentPartyData
        )
        {
            this.game = game;
            this.agentPartyData = agentPartyData;

            game.agentSpawned.AddListener(OnAgentSpawned);
        }

        void OnAgentSpawned(Agent agent, Vector3 pos, Quaternion rot)
        {
            if (agent.partyMember.AgentPartyData != agentPartyData)
            {
                enemies.Add(agent);

                if (agent.health.isAlive) aliveEnemies.Add(agent);
            }

            agent.partyMember.changedParty.AddListener(OnAgentChangedParty);
            agent.health.died.AddListener(OnAgentDied);
        }

        void OnAgentChangedParty(Agent agent)
        {
            // become enemy
            // become ally

            if (enemies.Contains(agent) == false && agent.partyMember.AgentPartyData != agentPartyData)
            {
                enemies.Add(agent);

                if (agent.health.isAlive) aliveEnemies.Add(agent);
            }
            else if (enemies.Contains(agent) && agent.partyMember.AgentPartyData == agentPartyData)
            {
                enemies.Remove(agent);

                if (aliveEnemies.Contains(agent)) aliveEnemies.Remove(agent);
            }
        }

        void OnAgentDied(Agent agent)
        {
            if (aliveEnemies.Contains(agent)) aliveEnemies.Remove(agent);
        }
    }
}