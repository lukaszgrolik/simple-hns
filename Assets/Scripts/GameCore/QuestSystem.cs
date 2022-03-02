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

    public class QuestTaskKillEnemies : QuestTask
    {
        public readonly List<Agent> remainingAgents;

        public QuestTaskKillEnemies(List<Agent> remainingAgents = null)
        {
            this.remainingAgents = remainingAgents != null ? remainingAgents : new List<Agent>();

            for (int i = 0; i < this.remainingAgents.Count; i++)
            {
                this.remainingAgents[i].health.died += OnAgentDied;
            }
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

    public class QuestTaskEnterLocation : QuestTask
    {

    }

    public class QuestTaskCollectItem : QuestTask
    {

    }

    public class QuestTaskDeliverItem : QuestTask
    {

    }

    public class QuestTaskMeetAgent : QuestTask
    {

    }

    public enum QuestStatus
    {
        Unstarted,
        InProgress,
        Finished
    }

    public class Quest
    {
        private string title;
        private string description;
        public readonly List<QuestTask> tasks;
        private QuestStatus status = QuestStatus.Unstarted;
        private QuestTask activeTask;

        public event System.Action statusChanged;
        public event System.Action activeTaskChanged;

        public Quest(
            string title,
            string description,
            List<QuestTask> tasks
        )
        {
            this.tasks = tasks;

            for (int i = 0; i < tasks.Count; i++)
            {
                tasks[i].criteriaMet += OnTaskCriteriaMet;
            }
        }

        public void Start()
        {
            status = QuestStatus.InProgress;
            activeTask = tasks[0];

            statusChanged?.Invoke();
            activeTaskChanged?.Invoke();

            Debug.Log("quest started");
        }

        void OnTaskCriteriaMet(QuestTask task)
        {
            var index = tasks.IndexOf(task);

            // remove listeners from previous tasks - should guard from processing earlier tasks if a later one has been finished
            for (int i = 0; i < index + 1; i++)
            {
                tasks[i].criteriaMet -= OnTaskCriteriaMet;
            }

            if (index == tasks.Count - 1)
            {
                status = QuestStatus.Finished;

                statusChanged?.Invoke();

                Debug.Log("quest finished");
            }
            else
            {
                activeTask = tasks[index + 1];

                activeTaskChanged?.Invoke();

                Debug.Log("quest moved to next task");
            }
        }
    }

    public class QuestSystem
    {
        public readonly List<Quest> quests;

        public QuestSystem(List<Quest> quests)
        {
            this.quests = quests;
        }
    }
}