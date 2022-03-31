using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonoBehaviors
{
    public class LocationsManager : MonoBehaviour
    {
        [SerializeField] private GameplayManager gameplayManager;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private CameraFollow cameraFollow;

        [SerializeField] private List<Transform> waypoints;

        private List<KeyCode> keys = new List<KeyCode>(){
            KeyCode.F1,
            KeyCode.F2,
            KeyCode.F3,
            KeyCode.F4,
            KeyCode.F5,
            KeyCode.F6,
            KeyCode.F7,
            KeyCode.F8,
        };

        void Awake()
        {
            playerController.waypointModeOn += OnWaypointModeOn;
            playerController.waypointModeOff += OnWaypointModeOff;

            enabled = false;
        }

        void Update()
        {
            for (int i = 0; i < keys.Count; i++)
            {
                if (Input.GetKeyUp(keys[i]))
                {
                    StartCoroutine(gameplayManager.ControlledAgent.ForceUpdatePosition(waypoints[i].position, cameraFollow));
                }
            }
        }

        void OnWaypointModeOn()
        {
            enabled = true;
        }

        void OnWaypointModeOff()
        {
            enabled = false;
        }


    }
}
