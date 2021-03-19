using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class GameplayManager : MonoBehaviour {
    [SerializeField] private Camera cam;
    public Camera Cam => cam;

    [SerializeField] private LayerMask groundLayerMask;
    public LayerMask GroundLayerMask => groundLayerMask;

    [SerializeField] private AgentController controlledAgent;
    public AgentController ControlledAgent => controlledAgent;

    void Start() {
        var playerController = GetComponent<PlayerController>();
        playerController.Setup(this);

        var cameraFollow = GetComponent<CameraFollow>();
        cameraFollow.Setup(this, controlledAgent.transform);

        var agents = FindObjectsOfType<AgentController>();
        foreach (var agent in agents) {
            agent.Setup();
        }
    }
}
