using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour {
    private PlayerController playerController;

    private AgentsManager agentsManager;
    public AgentsManager AgentsManager => agentsManager;

    [SerializeField] private GameUI gameUI;

    [SerializeField] private Camera cam;
    public Camera Cam => cam;

    [SerializeField] private LayerMask groundLayerMask;
    public LayerMask GroundLayerMask => groundLayerMask;

    [SerializeField] private GameObject movementTargetPrefab;
    public GameObject MovementTargetPrefab => movementTargetPrefab;

    [SerializeField] private Transform movementTargetContainer;
    public Transform MovementTargetContainer => movementTargetContainer;

    [SerializeField] private GameObject projectilePrefab;
    public GameObject ProjectilePrefab => projectilePrefab;

    [SerializeField] private Transform projectilesContainer;
    public Transform ProjectilesContainer => projectilesContainer;

    [SerializeField] private AgentController controlledAgent;
    public AgentController ControlledAgent => controlledAgent;

    private SerializationManager serializationManager = new SerializationManager();
    public SerializationManager SerializationManager => serializationManager;

    private SaveData saveData;
    public SaveData SaveData => saveData;

    void Start() {
        playerController = GetComponent<PlayerController>();
        playerController.Setup(this);

        var cameraFollow = GetComponent<CameraFollow>();
        cameraFollow.Setup(this, controlledAgent.transform);

        agentsManager = GetComponent<AgentsManager>();
        agentsManager.Setup();

        serializationManager.Setup();
        Load();

        var agents = FindObjectsOfType<AgentController>();
        foreach (var agent in agents) {
            if (agent == controlledAgent) {
                agent.Setup(this, agentsManager.Parties[0]);
                // @todo update controlledAgent health from saveData
            }
            else {
                agent.Setup(this, agentsManager.Parties[1]);
            }

            agentsManager.RegisterAgent(agent);
        }

        gameUI.SetPlayerHealth(controlledAgent.Health.CurrentPoints, controlledAgent.Health.MaxPoints);

        controlledAgent.Health.HealthChanged.AddListener(OnControlledAgentHealthPointsChanged);
    }

    void Load() {
        var fileData = (SaveData)serializationManager.Load("character1");
        saveData = fileData != null ? fileData : new SaveData();
    }

    public void Save() {
        serializationManager.Save("character1", saveData);
    }

    void OnControlledAgentHealthPointsChanged(int currentPoints, int maxPoints) {
        gameUI.SetPlayerHealth(currentPoints, maxPoints);
    }
}
