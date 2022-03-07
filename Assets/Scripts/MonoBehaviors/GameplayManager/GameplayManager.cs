using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviors
{
    [System.Serializable]
    public enum AgentType
    {
        Hero,
        Demon,
        Warden,
    }

    [System.Serializable]
    public enum ItemType
    {
        Sword, Axe, Staff,
        Helm, Shield
    }

    public class GameplayManager : MonoBehaviour
    {
        // public static readonly List<DataDefinition.Agent> enemies = new List<DataDefinition.Agent>()
        // {
        //     DataInstance.Agents.demon,
        //     DataInstance.Agents.zombie,
        //     DataInstance.Agents.skeleton,
        //     DataInstance.Agents.skeletonArcher,
        //     DataInstance.Agents.skeletonMage,
        // };

        private GameplayManagerDroppedItemEntities droppedItemEntities;
        private GameplayManagerProjectileEntities projectileEntities;
        private GameplayManagerAgentEntities agentEntities;

        [SerializeField] private GameUI gameUI;

        private GameplayManagerGameUI gameplayManagerGameUI;

        [SerializeField] private Camera cam;
        public Camera Cam => cam;

        [Header("Masks")]

        [SerializeField] private LayerMask groundLayerMask;
        public LayerMask GroundLayerMask => groundLayerMask;

        [SerializeField] private LayerMask agentLayerMask;
        public LayerMask AgentLayerMask => agentLayerMask;

        [SerializeField] private LayerMask droppedItemLayerMask;
        public LayerMask DroppedItemLayerMask => droppedItemLayerMask;

        [Header("Prefabs")]

        [SerializeField] private GameObject movementTargetPrefab;
        public GameObject MovementTargetPrefab => movementTargetPrefab;

        [SerializeField] private GameObject agentPrefab;
        public GameObject AgentPrefab => agentPrefab;

        [SerializeField] private GameObject droppedItemPrefab;
        public GameObject DroppedItemPrefab => droppedItemPrefab;

        [SerializeField] private GameObject projectilePrefab;
        public GameObject ProjectilePrefab => projectilePrefab;

        [Header("Containers")]

        [SerializeField] private Transform movementTargetContainer;
        public Transform MovementTargetContainer => movementTargetContainer;

        [SerializeField] private Transform agentsContainer;
        public Transform AgentsContainer => agentsContainer;

        [SerializeField] private Transform droppedItemsContainer;
        public Transform DroppedItemsContainer => droppedItemsContainer;

        [SerializeField] private Transform projectilesContainer;
        public Transform ProjectilesContainer => projectilesContainer;

        [Space(20)]

        [SerializeField] private GameObject controlledAgentObject;
        public GameObject ControlledAgentObject => controlledAgentObject;

        [System.Serializable]
        public class AgentModelPrefab
        {
            [SerializeField] private AgentType agentType;
            public AgentType AgentType => agentType;

            [SerializeField] private GameObject prefab;
            public GameObject Prefab => prefab;
        }

        [SerializeField] private List<AgentModelPrefab> agentModelPrefabs;

        [System.Serializable]
        public class ItemModelPrefab
        {
            [SerializeField] private ItemType itemType;
            public ItemType ItemType => itemType;

            [SerializeField] private GameObject prefab;
            public GameObject Prefab => prefab;
        }

        [SerializeField] private List<ItemModelPrefab> itemModelPrefabs;

        //
        //
        //
        //
        //

        private SerializationManager serializationManager = new SerializationManager();
        public SerializationManager SerializationManager => serializationManager;

        private SaveData saveData;
        public SaveData SaveData => saveData;

        // private readonly Dictionary<GameObject, GameCore.ITransformScript> objectScripts = new Dictionary<GameObject, GameCore.ITransformScript>();
        // public IReadOnlyDictionary<GameObject, GameCore.ITransformScript> ObjectScripts => objectScripts;

        // private readonly Dictionary<GameObject, GameCore.ITransformScript> dict_object_transformScript = new Dictionary<GameObject, GameCore.ITransformScript>();
        // public IReadOnlyDictionary<GameObject, GameCore.ITransformScript> Dict_object_transformScript => dict_object_transformScript;

        public readonly Dictionary<GameCore.ITransformScript, GameObject> dict_transformScript_object = new Dictionary<GameCore.ITransformScript, GameObject>();
        public IReadOnlyDictionary<GameCore.ITransformScript, GameObject> Dict_transformScript_object => dict_transformScript_object;

        public readonly Dictionary<GameObject, AgentController> dict_object_agentCtrl = new Dictionary<GameObject, AgentController>();
        public IReadOnlyDictionary<GameObject, AgentController> Dict_object_agentCtrl => dict_object_agentCtrl;

        public readonly Dictionary<GameCore.Agent, AgentController> dict_agent_agentCtrl = new Dictionary<GameCore.Agent, AgentController>();
        public IReadOnlyDictionary<GameCore.Agent, AgentController> Dict_agent_agentController => dict_agent_agentCtrl;

        private AgentController controlledAgent;
        public AgentController ControlledAgent => controlledAgent;

        private Game game;
        public Game Game => game;

        private GameCore.AgentsParty heroParty;
        private GameCore.AgentsParty neutralParty;
        private GameCore.AgentsParty monsterParty;

        void Start()
        {
            this.heroParty = new GameCore.AgentsParty(
                game: game,
                agentPartyData: new DataDefinition.AgentParty("good party")
            );
            this.neutralParty = new GameCore.AgentsParty(
                game: game,
                agentPartyData: new DataDefinition.AgentParty("neutral party")
            );
            this.monsterParty = new GameCore.AgentsParty(
                game: game,
                agentPartyData: new DataDefinition.AgentParty("evil party")
            );

            this.droppedItemEntities = new GameplayManagerDroppedItemEntities(
                gameplayManager: this,
                droppedItemPrefab: droppedItemPrefab,
                droppedItemsContainer: droppedItemsContainer,
                itemModelPrefabs: itemModelPrefabs
            );
            this.projectileEntities = new GameplayManagerProjectileEntities(
                gameplayManager: this,
                projectilePrefab: projectilePrefab,
                projectilesContainer: projectilesContainer
            );
            this.agentEntities = new GameplayManagerAgentEntities(
                gameplayManager: this,
                agentPrefab: agentPrefab,
                agentsContainer: agentsContainer,
                agentModelPrefabs: agentModelPrefabs,
                heroParty: this.heroParty,
                neutralParty: this.neutralParty,
                monsterParty: this.monsterParty
            );
            this.gameplayManagerGameUI = new GameplayManagerGameUI(this, gameUI);

            serializationManager.Setup();
            Load();

            this.game = new Game(
                gameplayManager: this,
                quests: new List<GameCore.Quest>(){
                    new GameCore.Quest(
                        title: "Bla bla",
                        description: "some desc",
                        tasks: new List<GameCore.QuestTask>(){
                            new GameCore.QuestTaskKillEnemies()
                        }
                    )
                },
                agentsParties: new List<GameCore.AgentsParty>(){
                    this.heroParty,
                    this.neutralParty,
                    this.monsterParty,
                },
                enemyParties: new List<GameCore.AgentsParty>(){
                    this.heroParty,
                    this.monsterParty,
                }
            );
            game.itemSystem.itemSpawned += droppedItemEntities.OnDroppedItemSpawned;
            game.itemSystem.itemDeleted += droppedItemEntities.OnDroppedItemDeleted;
            game.projectileSpawned += projectileEntities.OnProjectileSpawned;
            game.projectileDeleted += projectileEntities.OnProjectileDeleted;
            game.agentSpawned += agentEntities.OnAgentSpawned;

            var sceneAgents = FindObjectsOfType<SceneAgent>();
            foreach (var sceneAgent in sceneAgents)
            {
                var agent = agentEntities.CreateAgent(game, sceneAgent.AgentType);
                game.SpawnAgent(agent, sceneAgent.transform.position, Quaternion.identity);

                if (controlledAgentObject == sceneAgent.gameObject)
                {
                    controlledAgent = dict_agent_agentCtrl[agent];
                    gameplayManagerGameUI.SetControlledAgent(controlledAgent);
                }

                Destroy(sceneAgent.gameObject);
            }

            gameplayManagerGameUI.Init();

            //
            //
            //

            var firstQuest = game.questSystem.quests[0];
            var firstTask = firstQuest.tasks[0] as GameCore.QuestTaskKillEnemies;
            firstTask.AddAgents(controlledAgent.Agent.partyMember.AgentsParty.enemies);
            firstQuest.Start();
        }

        void Update()
        {
            if (game == null) return;

            game.OnUpdate(Time.deltaTime);
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
    }
}
