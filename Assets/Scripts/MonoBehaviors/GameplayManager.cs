using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviors
{
    public class GameplayManager : MonoBehaviour
    {
        public static readonly List<DataDefinition.Agent> enemies = new List<DataDefinition.Agent>()
        {
            DataInstance.Agents.demon,
            DataInstance.Agents.zombie,
            DataInstance.Agents.skeleton,
            DataInstance.Agents.skeletonArcher,
            DataInstance.Agents.skeletonMage,
        };

        private PlayerController playerController;

        // private AgentsManager agentsManager;
        // public AgentsManager AgentsManager => agentsManager;

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

        [System.Serializable]
        class AgentPrefab
        {
            [SerializeField] private AgentType agentType;
            public AgentType AgentType => agentType;

            [SerializeField] private GameObject prefab;
            public GameObject Prefab => prefab;
        }

        [SerializeField] private List<AgentPrefab> agentPrefabs;

        //
        //
        //
        //
        //

        private SerializationManager serializationManager = new SerializationManager();
        public SerializationManager SerializationManager => serializationManager;

        private SaveData saveData;
        public SaveData SaveData => saveData;

        private readonly List<GameCore.Agent> agents = new List<GameCore.Agent>();

        private readonly Dictionary<GameObject, AgentController> agentObjectsControllers = new Dictionary<GameObject, AgentController>();
        public IReadOnlyDictionary<GameObject, AgentController> AgentObjectsControllers => agentObjectsControllers;

        void Start()
        {
            playerController = GetComponent<PlayerController>();
            playerController.Setup(this);

            var cameraFollow = GetComponent<CameraFollow>();
            cameraFollow.Setup(this, controlledAgent.transform);

            // agentsManager = GetComponent<AgentsManager>();
            // agentsManager.Setup();

            serializationManager.Setup();
            Load();

            var agentMBs = FindObjectsOfType<AgentController>();
            foreach (var agentMB in agentMBs)
            {
                // if (agent == controlledAgent) {
                //     agent.Setup(this, agentsManager.Parties[0]);
                //     // @todo update controlledAgent health from saveData
                // }
                // else {
                //     agent.Setup(this, agentsManager.Parties[1]);
                // }

                // agentsManager.RegisterAgent(agent);

                var (agent, agentObj, agentCtrl) = SetupAgent(agentMB);

                agents.Add(agent);
                agentObjectsControllers.Add(agentObj, agentCtrl);
            }

            var ctrlAgentHealth = controlledAgent.Agent.health;

            gameUI.SetPlayerHealth(ctrlAgentHealth.CurrentPoints, ctrlAgentHealth.MaxPoints);

            ctrlAgentHealth.healthChanged.AddListener(OnControlledAgentHealthPointsChanged);
        }

        void Update()
        {
            for (int i = 0; i < agents.Count; i++)
                agents[i].OnUpdate(Time.deltaTime);
        }

        (GameCore.Agent, GameObject, AgentController) SetupAgent(AgentController agentMB)
        {
            DataDefinition.Agent agentData = null;
            if (agentMB.AgentType == AgentType.Hero)
                agentData = DataInstance.Agents.hero;
            else if (agentMB.AgentType == AgentType.Demon)
                agentData = DataInstance.Agents.demon;

            var agentMovement = new GameCore.AgentMovement();
            var agent = new GameCore.Agent(
                health: new GameCore.AgentHealth(),
                movement: agentMovement,
                combat: new GameCore.AgentCombat(
                    movement: agentMovement
                ),
                agentData: agentData
            );

            agentMB.Setup(
                gameplayManager: this,
                agent: agent
            );

            var agentPrefab = agentPrefabs.Find(p => p.AgentType == agentMB.AgentType);
            if (agentPrefab == null) throw new System.Exception($"agent prefab not found: {agentMB.AgentType}");

            var agentObj = Instantiate(agentPrefab.Prefab, agentMB.transform);
            var agentCtrl = agentObj.GetComponent<AgentController>();

            var spriteModel = agentObj.GetComponentInChildren<SpriteModel>();
            if (spriteModel == null) throw new System.Exception($"agent SpriteModel not found: {agentMB.AgentType}");

            // @todo magic number
            spriteModel.transform.rotation = Quaternion.Euler(37.5f, 45f, 0f);

            // @todo reassign values before removing collider and navMeshAgent

            var modelCollider = agentObj.GetComponent<Collider>();
            Destroy(modelCollider);

            var modelNavMeshAgent = agentObj.GetComponent<NavMeshAgent>();
            Destroy(modelNavMeshAgent);

            Destroy(spriteModel);

            return (agent, agentObj, agentCtrl);
        }

        void Load()
        {
            var fileData = (SaveData)serializationManager.Load("character1");
            saveData = fileData != null ? fileData : new SaveData();
        }

        public void Save()
        {
            serializationManager.Save("character1", saveData);
        }

        void OnControlledAgentHealthPointsChanged(int currentPoints, int maxPoints)
        {
            gameUI.SetPlayerHealth(currentPoints, maxPoints);
        }
    }
}
