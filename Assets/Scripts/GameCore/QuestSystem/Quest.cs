using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public enum QuestStatus
    {
        Unstarted,
        InProgress,
        Finished
    }

    public class Quest
    {
        // private string title;
        // public string Title => title;

        // private string description;
        // public string Description => description;

        public readonly DataDefinition.Quest data;

        public readonly List<QuestTask> tasks = new List<QuestTask>();

        private QuestStatus status = QuestStatus.Unstarted;
        public QuestStatus Status => status;

        private QuestTask activeTask;
        public QuestTask ActiveTask => activeTask;

        public event System.Action statusChanged;
        public event System.Action activeTaskChanged;

        public Quest(
            Game game,
            DataDefinition.Quest data
            // List<QuestTask> tasks
        )
        {
            this.data = data;
            // this.tasks = tasks;

            for (int i = 0; i < data.tasks.Count; i++)
            {
                var taskData = data.tasks[i];

                QuestTask task = null;

                     if (taskData is DataDefinition.QuestTask_KillAllInLocation taskData_killAllInLocation) task = new QuestTask_KillAllInLocation(taskData_killAllInLocation);
                else if (taskData is DataDefinition.QuestTask_KillEnemy taskData_killEnemy) task = new QuestTask_KillEnemy();
                else if (taskData is DataDefinition.QuestTask_EnterLocation taskData_enterLocation) task = new QuestTask_EnterLocation(taskData_enterLocation, game);
                else if (taskData is DataDefinition.QuestTask_EnterAgentProximity taskData_enterAgentProximity) task = new QuestTask_EnterAgentProximity();
                else if (taskData is DataDefinition.QuestTask_CollectItem taskData_collectItem) task = new QuestTask_CollectItem();
                else if (taskData is DataDefinition.QuestTask_DeliverItem taskData_deliverItem) task = new QuestTask_DeliverItem();
                else if (taskData is DataDefinition.QuestTask_MeetAgent taskData_meetAgent) task = new QuestTask_MeetAgent();
                else
                {
                    throw new System.Exception($"Unsupported quest task data type: {taskData.ToString()}");
                }

                tasks.Add(task);
            }

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
}