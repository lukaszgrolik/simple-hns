using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace GameCore
{
    public class AgentsParty
    {
        public readonly Game game;

        private DataDefinition.AgentParty agentPartyData;
        public DataDefinition.AgentParty AgentPartyData => agentPartyData;

        public readonly List<Agent> enemies = new List<Agent>();
        public readonly List<Agent> aliveEnemies = new List<Agent>();

        public AgentsParty(
            Game game,
            DataDefinition.AgentParty agentPartyData
        )
        {
            this.game = game;
            this.agentPartyData = agentPartyData;

            game.agentSpawned += OnAgentSpawned;
        }

        public bool IsEnemy(Agent agent)
        {
            return enemies.Contains(agent);
        }

        public bool IsAliveEnemy(Agent agent)
        {
            return aliveEnemies.Contains(agent);
        }

        void OnAgentSpawned(Agent agent, Vector3 pos, Quaternion rot)
        {
            if (agent.partyMember.AgentsParty != this)
            {
                enemies.Add(agent);

                if (agent.health.isAlive) aliveEnemies.Add(agent);
            }

            agent.partyMember.changedParty += OnAgentChangedParty;
            agent.health.died += OnAgentDied;
        }

        void OnAgentChangedParty(Agent agent)
        {
            // become enemy
            // become ally

            if (enemies.Contains(agent) == false && agent.partyMember.AgentsParty != this)
            {
                enemies.Add(agent);

                if (agent.health.isAlive) aliveEnemies.Add(agent);
            }
            else if (enemies.Contains(agent) && agent.partyMember.AgentsParty == this)
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