using UnityEngine;
using UnityEngine.Events;

namespace GameCore
{
    public class AgentMovement
    {
        private bool isChangingDestination = false;
        public bool IsChangingDestination => isChangingDestination;

        private Vector2 currentDestination;
        public Vector2 CurrentDestination => currentDestination;

        public class DestinationChangedEvent : UnityEvent<Vector2> { }
        public readonly DestinationChangedEvent destinationChanged = new DestinationChangedEvent();

        public class CancelledEvent : UnityEvent { }
        public readonly CancelledEvent cancelled = new CancelledEvent();

        public class ArrivedEvent : UnityEvent { }
        public readonly ArrivedEvent arrived = new ArrivedEvent();

        public void SetDestination(Vector2 pos)
        {
            currentDestination = pos;
            isChangingDestination = true;

            destinationChanged.Invoke(pos);
        }

        public void Cancel()
        {
            isChangingDestination = false;

            cancelled.Invoke();
        }

        public void MarkArrived()
        {
            isChangingDestination = false;

            arrived.Invoke();
        }
    }
}