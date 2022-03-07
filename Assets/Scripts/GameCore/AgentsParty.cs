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

        bool IsSpawnedAgentEnemy(Agent agent)
        {
            if (game.EnemyParties.Contains(this) == false) return false;
            if (game.EnemyParties.Contains(agent.partyMember.AgentsParty) == false) return false;

            return agent.partyMember.AgentsParty != this;
        }

        void OnAgentSpawned(Agent agent, Vector3 pos, Quaternion rot)
        {
            if (IsSpawnedAgentEnemy(agent))
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

            if (enemies.Contains(agent) == false && IsSpawnedAgentEnemy(agent))
            {
                enemies.Add(agent);

                if (agent.health.isAlive) aliveEnemies.Add(agent);
            }
            else if (enemies.Contains(agent) && IsSpawnedAgentEnemy(agent) == false)
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