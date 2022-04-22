using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public abstract class QuestTask
    {
        public event System.Action<QuestTask> criteriaMet;

        protected void MarkCriteriaMet()
        {
            criteriaMet?.Invoke(this);
        }
    }

    public class QuestTask_KillAllInLocation : QuestTask
    {
        public readonly DataDefinition.QuestTask_KillAllInLocation data;
        public readonly List<Agent> remainingAgents;

        // public QuestTask_KillAllInLocation(List<Agent> remainingAgents = null)
        public QuestTask_KillAllInLocation(DataDefinition.QuestTask_KillAllInLocation data)
        {
            this.data = data;
            // this.remainingAgents = remainingAgents != null ? remainingAgents : new List<Agent>();

            // for (int i = 0; i < this.remainingAgents.Count; i++)
            // {
            //     this.remainingAgents[i].health.died += OnAgentDied;
            // }
        }

        public void SetLocation(Location location)
        {

        }

        public void AddAgent(Agent agent)
        {
            remainingAgents.Add(agent);

            agent.health.died += OnAgentDied;
        }

        public void AddAgents(List<Agent> agents)
        {
            remainingAgents.AddRange(agents);

            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].health.died += OnAgentDied;
            }
        }

        void OnAgentDied(Agent agent)
        {
            agent.health.died -= OnAgentDied;

            remainingAgents.Remove(agent);

            Debug.Log($"remainingAgents: {remainingAgents.Count}");

            if (remainingAgents.Count == 0)
            {
                Debug.Log($"task done");
                MarkCriteriaMet();
            }
        }
    }

    public class QuestTask_KillEnemy : QuestTask
    {

    }

    public class QuestTask_EnterLocation : QuestTask
    {
        public readonly DataDefinition.QuestTask_EnterLocation data;
        public Game game;

        private Location location;

        public QuestTask_EnterLocation(DataDefinition.QuestTask_EnterLocation data, Game game)
        {
            this.data = data;
            this.game = game;
        }

        public void SetLocation(Location location)
        {
            this.location = location;

            game.playerEnteredLocation += OnPlayerEnteredLocation;
        }

        void OnPlayerEnteredLocation(Location loc)
        {
            if (loc == location)
            {
                game.playerEnteredLocation -= OnPlayerEnteredLocation;

                MarkCriteriaMet();
            }
        }
    }

    public class QuestTask_EnterAgentProximity : QuestTask
    {

    }

    public class QuestTask_CollectItem : QuestTask
    {

    }

    public class QuestTask_DeliverItem : QuestTask
    {

    }

    public class QuestTask_MeetAgent : QuestTask
    {

    }
}