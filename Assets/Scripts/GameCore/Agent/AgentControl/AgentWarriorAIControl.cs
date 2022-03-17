using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public sealed class AgentWarriorAIControl : AgentControl, IAgentControlTickable
    {
        private SM.StateMachine sm = new SM.StateMachine();

        public override void Setup()
        {
            agent.health.died += OnAgentDied;
            agent.agentDetection.enemyDetected += OnEnemyDetected;
            agent.agentDetection.enemyLost += OnEnemyLost;

            HandleEnemiesNumberChange();
        }

        public void OnUpdate()
        {
            if (sm.State != null && sm.State is IAgentAITickableState state)
            {
                state.OnUpdate();
            }
        }

        void OnEnemyDetected(Agent agent)
        {
            HandleEnemiesNumberChange();
        }

        void OnEnemyLost(Agent agent)
        {
            HandleEnemiesNumberChange();
        }

        void HandleEnemiesNumberChange()
        {
            // Debug.Log($"aliveEnemies.Count: {agent.agentDetection.aliveEnemies.Count}");
            if (agent.agentDetection.aliveEnemies.Count > 0)
            {
                if (sm.State is AgentAI.States.Combat combatState)
                {
                    combatState.OnAliveEnemiesChanged(agent.agentDetection.aliveEnemies);
                }
                else
                {
                    sm.SetState(new AgentAI.States.Combat(agent));
                }
            }
            else
            {
                sm.SetState(new AgentAI.States.Patrol(agent));
            }
        }

        void OnAgentDied(Agent agent)
        {
            this.agent.agentDetection.enemyDetected -= OnEnemyDetected;
            this.agent.agentDetection.enemyLost -= OnEnemyLost;

            sm.Exit();
        }
    }
}