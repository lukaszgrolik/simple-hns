using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AgentDetection : AgentComponent
    {
        private float sightRadius;
        public float SightRadius => sightRadius;

        public readonly List<Agent> agents = new List<Agent>();

        public readonly List<Agent> enemies = new List<Agent>();
        public readonly List<Agent> aliveEnemies = new List<Agent>();

        public readonly List<Agent> allies = new List<Agent>();

        public event System.Action<Agent> enemyDetected;
        public event System.Action<Agent> enemyLost;

        public AgentDetection(float sightRadius)
        {
            this.sightRadius = sightRadius;
        }

        public void AddDetectedAgent(Agent otherAgent)
        {
            agents.Add(otherAgent);

            if (agent.partyMember.AgentsParty.enemies.Contains(otherAgent))
            {
                enemies.Add(otherAgent);

                if (otherAgent.health.isAlive)
                {
                    aliveEnemies.Add(otherAgent);
                }

                enemyDetected?.Invoke(otherAgent);
            }

            otherAgent.health.died += OnOtherAgentDied;
            otherAgent.partyMember.changedParty += OnOtherAgentChangedParty;
        }

        public void RemoveDetectedAgent(Agent otherAgent)
        {
            agents.Remove(otherAgent);

            if (agent.partyMember.AgentsParty.enemies.Contains(otherAgent))
            {
                enemies.Remove(otherAgent);

                if (aliveEnemies.Contains(otherAgent))
                {
                    aliveEnemies.Remove(otherAgent);
                }

                enemyLost?.Invoke(otherAgent);
            }

            otherAgent.health.died -= OnOtherAgentDied;
            otherAgent.partyMember.changedParty -= OnOtherAgentChangedParty;
        }

        void OnOtherAgentDied(Agent otherAgent)
        {
            aliveEnemies.Remove(otherAgent);
        }

        void OnOtherAgentChangedParty(Agent otherAgent)
        {
            // @todo
            // invoke enemy detected if changed to enemy party
            // invoke enemy lost if changed to non-enemy party
        }
    }
}