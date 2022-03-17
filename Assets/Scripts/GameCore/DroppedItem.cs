using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class DroppedItem : ITransformScript
    {
        public readonly Game game;
        public readonly Item item;

        public event System.Action picked;

        public DroppedItem(Game game, Item item)
        {
            this.game = game;
            this.item = item;
        }

        public Vector3 GetPosition()
        {
            return game.GetPosition(this);
        }
    }
}