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
            KeyCode.F9,
            KeyCode.F10,
            KeyCode.F11,
            KeyCode.F12,
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
                    var wp = waypoints[i];
                    if (wp.gameObject.activeInHierarchy)
                    {
                        StartCoroutine(gameplayManager.ControlledAgent.ForceUpdatePosition(wp.position, cameraFollow));
                    }
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

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            foreach (Transform wp in waypoints)
            {
                if (wp.gameObject.activeInHierarchy == false) continue;

                Gizmos.color = Color.red;
                Gizmos.DrawSphere(wp.position, .25f);
                Gizmos.DrawLine(wp.position, wp.position + Vector3.up * 3f);
            }
        }
#endif
    }
}
