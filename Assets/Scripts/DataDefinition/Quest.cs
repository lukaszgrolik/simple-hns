using System.Collections;
using System.Collections.Generic;

namespace DataDefinition
{
    public abstract class QuestTask
    {
        public readonly string info;

        public QuestTask(string info = null)
        {
            this.info = info;
        }
    }

    public class QuestTask_KillAllInLocation : QuestTask
    {
        public readonly Location location;
        public readonly System.Func<List<GameCore.Agent>, string> infoFn;

        public QuestTask_KillAllInLocation(
            Location location,
            System.Func<List<GameCore.Agent>, string> infoFn
        )
        {
            this.location = location;
            this.infoFn = infoFn;
        }
    }

    public class QuestTask_KillEnemy : QuestTask
    {
        public readonly Agent agent;

        public QuestTask_KillEnemy(
            Agent agent
        )
        {
            this.agent = agent;
        }
    }

    public class QuestTask_EnterLocation : QuestTask
    {
        public readonly Location location;

        public QuestTask_EnterLocation(
            string info,
            Location location
        ) : base(info)
        {
            this.location = location;
        }
    }

    public class QuestTask_EnterAgentProximity : QuestTask
    {
        public readonly Agent agent;

        public QuestTask_EnterAgentProximity(
            string info,
            Agent agent
        ) : base(info)
        {
            this.agent = agent;
        }
    }

    public class QuestTask_CollectItem : QuestTask
    {
        public readonly Item item;

        public QuestTask_CollectItem(
            string info,
            Item item
        ) : base(info)
        {
            this.item = item;
        }
    }

    public class QuestTask_DeliverItem : QuestTask { }

    public class QuestTask_MeetAgent : QuestTask { }

    public class Quest
    {
        public readonly string title;
        // public readonly string description;
        public readonly List<QuestTask> tasks;

        public Quest(
            string title,
            // string description,
            List<QuestTask> tasks
        )
        {
            this.title = title;
            // this.description = description;
            this.tasks = tasks;
        }
    }
}