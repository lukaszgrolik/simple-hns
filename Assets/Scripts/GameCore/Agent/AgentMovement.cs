using UnityEngine;

namespace GameCore
{
    public class AgentMovement
    {
        private bool isChangingDestination = false;
        public bool IsChangingDestination => isChangingDestination;

        private Vector2 currentDestination;
        public Vector2 CurrentDestination => currentDestination;

        public event System.Action<Vector2> destinationChanged;
        public event System.Action cancelled;
        public event System.Action arrived;

        public void SetDestination(Vector2 pos)
        {
            currentDestination = pos;
            isChangingDestination = true;

            destinationChanged.Invoke(pos);
        }

        public void Cancel()
        {
            isChangingDestination = false;

            cancelled?.Invoke();
        }

        public void MarkArrived()
        {
            isChangingDestination = false;

            arrived?.Invoke();
        }
    }
}