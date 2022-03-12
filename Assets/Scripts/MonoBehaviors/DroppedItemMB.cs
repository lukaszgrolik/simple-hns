using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonoBehaviors
{
    public class DroppedItemMB : MonoBehaviour
    {
        private GameplayManager gameplayManager;

        private GameCore.DroppedItem droppedItem;
        public GameCore.DroppedItem DroppedItem => droppedItem;

        public void Setup(
            GameplayManager gameplayManager,
            GameCore.DroppedItem droppedItem
        )
        {
            this.gameplayManager = gameplayManager;
            this.droppedItem = droppedItem;
        }
    }
}
