using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class DroppedItem : ITransformScript
    {
        public readonly Item item;

        public event System.Action picked;

        public DroppedItem(Item item)
        {
            this.item = item;
        }
    }
}