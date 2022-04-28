using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviors
{
    [System.Serializable]
    public enum ProjectileType
    {
        FireBolt,
        FireBall,
        EnergyBolt,
        SkeletonFireBolt,
        HoodedWarriorMageEnergyBolt,
        ButterflyEnergyBolt,
        BulbfrogEnergyBolt,
    }

    [System.Serializable]
    public enum AgentType
    {
        Hero,
        Demon,
        Warden,
        Warrior,
        HoodedWarrior,
        Bulbfrog,
        Ent,
        Skeleton,
        SkeletonArcher,
        SkeletonMage,
        Zombie,
        Butterfly,
        Crab,
        WalkingShroom,
        Demon_Orange,
        Demon_Blue,
        Demon_Black,
        Zombie_Desert,
        Zombie_Snow,
        HoodedWarriorMage,
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
        public GameUI GameUI => gameUI;

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

        [SerializeField] private GameObject projectileExplosionPrefab;
        public GameObject ProjectileExplosionPrefab => projectileExplosionPrefab;

        [Header("Containers")]

        [SerializeField] private Transform movementTargetContainer;
        public Transform MovementTargetContainer => movementTargetContainer;

        [SerializeField] private Transform agentsContainer;
        public Transform AgentsContainer => agentsContainer;

        [SerializeField] private Transform droppedItemsContainer;
        public Transform DroppedItemsContainer => droppedItemsContainer;

        [SerializeField] private Transform projectilesContainer;
        public Transform ProjectilesContainer => projectilesContainer;

        [SerializeField] private Transform projectileExplosionsContainer;
        public Transform ProjectileExplosionsContainer => projectileExplosionsContainer;

        [Space(20)]

        [SerializeField] private GameObject controlledAgentObject;
        public GameObject ControlledAgentObject => controlledAgentObject;

        // [System.Serializable]
        // public class Location
        // {
        //     private Collider boundaryCollider;
        //     private List<Transform> entryPoints;
        // }

        // [SerializeField] private List<Location> locations;

        [System.Serializable]
        public class ProjectileModelPrefab
        {
            [SerializeField] private ProjectileType projectileType;
            public ProjectileType ProjectileType => projectileType;

            [SerializeField] private GameObject prefab;
            public GameObject Prefab => prefab;
        }

        [SerializeField] private List<ProjectileModelPrefab> projectileModelPrefabs;

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
        // public IReadOnlyDictionary<GameCore.ITransformScript, GameObject> Dict_transformScript_object => dict_transformScript_object;

        public readonly Dictionary<GameObject, AgentController> dict_object_agentCtrl = new Dictionary<GameObject, AgentController>();
        // public IReadOnlyDictionary<GameObject, AgentController> Dict_object_agentCtrl => dict_object_agentCtrl;

        public readonly Dictionary<GameCore.Agent, AgentController> dict_agent_agentCtrl = new Dictionary<GameCore.Agent, AgentController>();
        // public IReadOnlyDictionary<GameCore.Agent, AgentController> Dict_agent_agentCtrl => dict_agent_agentCtrl;

        public readonly Dictionary<GameObject, DroppedItemMB> dict_object_droppedItemMB = new Dictionary<GameObject, DroppedItemMB>();
        // public IReadOnlyDictionary<GameObject, DroppedItemMB> Dict_object_droppedItemMB => dict_object_droppedItemMB;

        public readonly Dictionary<LocationMB, GameCore.Location> dict_locMb_loc = new Dictionary<LocationMB, GameCore.Location>();
        public readonly Dictionary<DataDefinition.Location, GameCore.Location> dict_locData_loc = new Dictionary<DataDefinition.Location, GameCore.Location>();

        private AgentController controlledAgent;
        public AgentController ControlledAgent => controlledAgent;

        private DataStore dataStore = new DataStore();
        public DataStore DataStore => dataStore;

        private Game game;
        public Game Game => game;

        void Awake()
        {
            var locationMBs = FindObjectsOfType<LocationMB>();
            foreach (var locMB in locationMBs)
            {
                locMB.gameObject.SetActive(false);
            }
        }

        void Start()
        {
            // Debug.Log($"DataInstance.Skills.EnergyBolt: {DataInstance.Skills.EnergyBolt}");
            // Debug.Log($"DataInstance.Agents.SkeletonMage: {DataInstance.Agents.SkeletonMage}");
            // Debug.Log($"DataInstance.Agents.SkeletonMage.skills.Count: {DataInstance.Agents.SkeletonMage.skills.Count}");
            // Debug.Log($"DataInstance.Agents.SkeletonMage.skills[0]: {DataInstance.Agents.SkeletonMage.skills[0]}");
            // Debug.Log($"DataInstance.Agents.SkeletonMage.skills[0].name: {DataInstance.Agents.SkeletonMage.skills[0].name}");

            // var dataStore = new DataStore();
            // var hero = dataStore.agents[AgentType.Hero];
            // Debug.Log($"agent.name: {hero.name}");
            // Debug.Log($"agent.skills.Count: {hero.skills.Count}");
            // Debug.Log($"agent.skills[0]: {hero.skills[0]}");
            // Debug.Log($"agent.skills[0].name: {hero.skills[0].name}");

            this.gameplayManagerGameUI = new GameplayManagerGameUI(this, gameUI);

            this.gameplayManagerGameUI.Setup();

            var heroParty = new GameCore.AgentsParty(
                agentPartyData: new DataDefinition.AgentParty("good party")
            );
            var neutralParty = new GameCore.AgentsParty(
                agentPartyData: new DataDefinition.AgentParty("neutral party")
            );
            var monsterParty = new GameCore.AgentsParty(
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
                heroParty: heroParty,
                neutralParty: neutralParty,
                monsterParty: monsterParty
            );

            serializationManager.Setup();
            Load();

            this.game = new Game(
                gameplayManager: this
            );
            game.itemSystem.itemSpawned += droppedItemEntities.OnDroppedItemSpawned;
            game.itemSystem.itemDeleted += droppedItemEntities.OnDroppedItemDeleted;
            game.projectileSpawned += projectileEntities.OnProjectileSpawned;
            game.projectileDeleted += projectileEntities.OnProjectileDeleted;
            game.agentSpawned += agentEntities.OnAgentSpawned;
            game.playerEnteredLocation += (loc) => {
                Debug.Log($"player entering location: {loc.data.name}");
            };

            this.game.InitAgentsParties(
                agentsParties: new List<GameCore.AgentsParty>(){
                    heroParty,
                    neutralParty,
                    monsterParty,
                },
                enemyParties: new List<GameCore.AgentsParty>(){
                    heroParty,
                    monsterParty,
                }
            );

            Debug.Log("x");

            var locationMBs = FindObjectsOfType<LocationMB>(includeInactive: true);
            var locations = new List<GameCore.Location>();
            foreach (var locMB in locationMBs)
            {
                locMB.Setup(this);

                // Debug.Log($"locMB.LocationDataHandle: {locMB.LocationDataHandle}");
                var locData = dataStore.dict_locHandle_locData[locMB.LocationDataHandle];
                // Debug.Log($"locData: {locData.name}");
                var loc = new GameCore.Location(
                    locationSystem: game.locationSystem,
                    data: locData
                );

                locations.Add(loc);
                dict_locMb_loc.Add(locMB, loc);
                dict_locData_loc.Add(locData, loc);
            }

            game.SetLocations(locations);

            foreach (var locMB in locationMBs)
            {
                locMB.gameObject.SetActive(true);
            }

            var agentSpawners = FindObjectsOfType<AgentSpawner>();
            foreach (var agentSpawner in agentSpawners)
            {
                agentSpawner.SpawnAgents(
                    game: game,
                    agentEntities: agentEntities
                );

                Destroy(agentSpawner.gameObject);
            }

            var sceneAgents = FindObjectsOfType<SceneAgent>();
            foreach (var sceneAgent in sceneAgents)
            {
                var agent = agentEntities.CreateAgent(
                    game: game,
                    agentType: sceneAgent.AgentType,
                    agentsGroup: game.CreateAgentsGroup()
                );
                game.SpawnAgent(agent, sceneAgent.transform.position, Quaternion.identity);

                if (controlledAgentObject == sceneAgent.gameObject)
                {
                    controlledAgent = dict_agent_agentCtrl[agent];
                    game.SetPlayerAgent(agent);
                    gameplayManagerGameUI.SetControlledAgent(controlledAgent);
                }

                Destroy(sceneAgent.gameObject);
            }

            var quests = new List<GameCore.Quest>();
            foreach (var questData in dataStore.quests)
            {
                var quest = new GameCore.Quest(
                    data: questData,
                    game: game
                );

                quests.Add(quest);
            }

            game.SetQuests(quests);

            gameplayManagerGameUI.Init();

            //
            //
            //

            // var firstQuest = game.questSystem.quests[0];
            // var firstTask = firstQuest.tasks[0] as GameCore.QuestTask_KillAllInLocation;
            // firstTask.AddAgents(controlledAgent.Agent.partyMember.AgentsParty.enemies);
            // firstQuest.Start();
            // Debug.Log("A");
            // foreach (var quest in game.questSystem.quests)
            // {
            //     foreach (var task in quest.tasks)
            //     {
            //         if (task is GameCore.QuestTask_EnterLocation task_enterLoc)
            //         {
            //             var location = dict_locData_loc[task_enterLoc.data.location];
            //             task_enterLoc.SetLocation(location);
            //         }
            //         else if (task is GameCore.QuestTask_KillAllInLocation task_killAll)
            //         {
            //             var location = dict_locData_loc[task_killAll.data.location];
            //             task_killAll.SetLocation(location);

            //             var agents = new List<GameCore.Agent>();
            //             foreach (var locAgent in location.Agents)
            //             {
            //                 if (controlledAgent.Agent.partyMember.AgentsParty.IsEnemy(locAgent))
            //                 {
            //                     agents.Add(locAgent);
            //                 }
            //             }

            //             task_killAll.AddAgents(agents);
            //         }
            //     }

            //     quest.Start();
            // }
            // Debug.Log("B");
        }

        void Update()
        {
            if (game == null) return;

            game.OnUpdate(Time.deltaTime);
        }

        public GameObject FindAgentModelPrefab(AgentType agentType)
        {
            var agentModelPrefab = agentModelPrefabs.Find(p => p.AgentType == agentType);
            if (agentModelPrefab == null) throw new System.Exception($"agent prefab not found: {agentType}");

            return agentModelPrefab.Prefab;
        }

        public GameObject FindProjectileModelPrefab(ProjectileType projectileType)
        {
            var projectileModelPrefab = projectileModelPrefabs.Find(p => p.ProjectileType == projectileType);
            if (projectileModelPrefab == null) throw new System.Exception($"projectile prefab not found: {projectileType}");

            return projectileModelPrefab.Prefab;
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
