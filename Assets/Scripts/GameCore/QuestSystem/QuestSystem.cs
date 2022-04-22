using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class QuestSystem
    {
        public readonly List<Quest> quests = new List<Quest>();

        public QuestSystem()
        {

        }

        public void SetQuests(List<Quest> quests)
        {
            this.quests.AddRange(quests);
        }
    }
}