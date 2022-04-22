using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonoBehaviors
{
    public class LocationMB : MonoBehaviour
    {
        private GameplayManager gameplayManager;

        [SerializeField] private DataHandle.Location locationDataHandle;
        public DataHandle.Location LocationDataHandle => locationDataHandle;

        public void Setup(GameplayManager gameplayManager)
        {
            this.gameplayManager = gameplayManager;
        }

        // @todo @fix agents detection object gets collided but shouldn't
        void OnTriggerEnter(Collider info)
        {
            if (gameplayManager.dict_object_agentCtrl.TryGetValue(info.gameObject, out var agentCtrl))
            {
                var location = gameplayManager.dict_locMb_loc[this];
                // Debug.Log($"LocationMB location: {location.data.name}");

                location.OnAgentEntered(agentCtrl.Agent);
            }
            else
            {
                Debug.Log($"LocationMB OnTriggerEnter invalid object: {info.gameObject.name}");
            }
        }
    }
}
