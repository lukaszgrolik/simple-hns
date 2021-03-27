using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameplayManager : MonoBehaviour {
    [SerializeField] private Camera cam;
    public Camera Cam => cam;

    [SerializeField] private LayerMask groundLayerMask;
    public LayerMask GroundLayerMask => groundLayerMask;

    [SerializeField] private AgentController controlledAgent;
    public AgentController ControlledAgent => controlledAgent;

    private SerializationManager serializationManager = new SerializationManager();
    public SerializationManager SerializationManager => serializationManager;

    private SaveData saveData;
    public SaveData SaveData => saveData;

    void Start() {
        var playerController = GetComponent<PlayerController>();
        playerController.Setup(this);

        var cameraFollow = GetComponent<CameraFollow>();
        cameraFollow.Setup(this, controlledAgent.transform);

        var agentsManager = GetComponent<AgentsManager>();
        agentsManager.Setup();

        var agents = FindObjectsOfType<AgentController>();
        foreach (var agent in agents) {
            if (agent == controlledAgent) {
                agent.Setup(agentsManager.Parties[0]);
            }
            else {
                agent.Setup(agentsManager.Parties[1]);
            }
        }

        serializationManager.Setup();
        Load();
    }

    void Load() {
        var fileData = (SaveData)serializationManager.Load("character1");
        saveData = fileData != null ? fileData : new SaveData();
    }

    public void Save() {
        serializationManager.Save("character1", saveData);
    }
}
