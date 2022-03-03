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
        Demon
    }

    [System.Serializable]
    public enum ItemType
    {
        Sword, Axe, Staff,
        Helm, Shield
    }

    public class Game : GameCore.Game
    {
        private GameplayManager gameplayManager;

        public Game(GameplayManager gameplayManager, List<GameCore.Quest> quests) : base(quests)
        {
            this.gameplayManager = gameplayManager;
        }

        public override Vector3 GetPosition(GameCore.ITransformScript script)
        {
            return gameplayManager.Dict_transformScript_object[script].transform.position;
        }

        public override Vector3 GetProjectileSpawnPosition(GameCore.Agent agent)
        {
            var agentCtrl = gameplayManager.Dict_agent_agentController[agent];
            var projSpawnPoint = agentCtrl.GetComponentInChildren<AgentProjectileSpawnPoint>();

            return projSpawnPoint.transform.position;
        }

        public override List<GameCore.Agent> FindAgentsInRadius(Vector3 pos, float radius)
        {
            var agents = new List<GameCore.Agent>();
            var agentControllers = Utils.FindColliders<AgentController>(pos, radius);

            for (int i = 0; i < agentControllers.Count; i++)
            {
                agents.Add(agentControllers[i].Agent);
            }

            return agents;
        }
    }

    public class GameplayManagerGameUI
    {
        private GameUI gameUI;
        private AgentController controlledAgent;

        public GameplayManagerGameUI(GameUI gameUI)
        {
            this.gameUI = gameUI;
        }

        public void SetControlledAgent(AgentController agentController)
        {
            this.controlledAgent = agentController;
        }

        public void OnControlledAgentHealthPointsChanged(GameCore.Agent agent)
        {
            gameUI.SetPlayerHealth(agent.health.CurrentPoints, agent.health.MaxPoints);
        }

        public void OnPlayerAgentMouseEntered(GameCore.Agent agent)
        {
            if (controlledAgent.Agent.partyMember.AgentsParty.IsAliveEnemy(agent))
            {
                gameUI.ShowEnemyHealth(agent.agentData.Name, agent.health.CurrentPoints, agent.health.MaxPoints);

                agent.health.healthChanged += OnPlayerHoveredAgentHealthChanged;
            }
        }

        public void OnPlayerAgentMouseLeft(GameCore.Agent agent)
        {
            gameUI.HideEnemyHealth();

            agent.health.healthChanged -= OnPlayerHoveredAgentHealthChanged;
        }

        void OnPlayerHoveredAgentHealthChanged(GameCore.Agent agent)
        {
            if (agent.health.CurrentPoints == 0)
            {
                gameUI.HideEnemyHealth();

                agent.health.healthChanged -= OnPlayerHoveredAgentHealthChanged;
            }
            else
            {
                gameUI.UpdateEnemyHealth(agent.health.CurrentPoints, agent.health.MaxPoints);
            }
        }
    }

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

        private GameplayManagerGameUI gameplayManagerGameUI;

        [SerializeField] private Camera cam;
        public Camera Cam => cam;

        [SerializeField] private LayerMask groundLayerMask;
        public LayerMask GroundLayerMask => groundLayerMask;

        [SerializeField] private LayerMask agentLayerMask;
        public LayerMask AgentLayerMask => agentLayerMask;

        [SerializeField] private LayerMask droppedItemLayerMask;
        public LayerMask DroppedItemLayerMask => droppedItemLayerMask;

        [SerializeField] private GameObject movementTargetPrefab;
        public GameObject MovementTargetPrefab => movementTargetPrefab;

        [SerializeField] private Transform movementTargetContainer;
        public Transform MovementTargetContainer => movementTargetContainer;

        [SerializeField] private GameObject agentPrefab;
        public GameObject AgentPrefab => agentPrefab;

        [SerializeField] private GameObject droppedItemPrefab;
        public GameObject DroppedItemPrefab => droppedItemPrefab;

        [SerializeField] private GameObject projectilePrefab;
        public GameObject ProjectilePrefab => projectilePrefab;

        [SerializeField] private Transform agentsContainer;
        public Transform AgentsContainer => agentsContainer;

        [SerializeField] private Transform projectilesContainer;
        public Transform ProjectilesContainer => projectilesContainer;

        [SerializeField] private GameObject controlledAgentObject;
        public GameObject ControlledAgentObject => controlledAgentObject;

        [System.Serializable]
        class AgentModelPrefab
        {
            [SerializeField] private AgentType agentType;
            public AgentType AgentType => agentType;

            [SerializeField] private GameObject prefab;
            public GameObject Prefab => prefab;
        }

        [SerializeField] private List<AgentModelPrefab> agentModelPrefabs;

        [System.Serializable]
        class ItemModelPrefab
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

        private readonly Dictionary<GameCore.ITransformScript, GameObject> dict_transformScript_object = new Dictionary<GameCore.ITransformScript, GameObject>();
        public IReadOnlyDictionary<GameCore.ITransformScript, GameObject> Dict_transformScript_object => dict_transformScript_object;

        private readonly Dictionary<GameObject, AgentController> dict_object_agentCtrl = new Dictionary<GameObject, AgentController>();
        public IReadOnlyDictionary<GameObject, AgentController> Dict_object_agentCtrl => dict_object_agentCtrl;

        private readonly Dictionary<GameCore.Agent, AgentController> dict_agent_agentCtrl = new Dictionary<GameCore.Agent, AgentController>();
        public IReadOnlyDictionary<GameCore.Agent, AgentController> Dict_agent_agentController => dict_agent_agentCtrl;

        private AgentController controlledAgent;
        public AgentController ControlledAgent => controlledAgent;

        private Game game;
        public Game Game => game;

        private GameCore.AgentsParty heroParty;
        private GameCore.AgentsParty monsterParty;

        void Start()
        {
            // agentsManager = GetComponent<AgentsManager>();
            // agentsManager.Setup();

            this.gameplayManagerGameUI = new GameplayManagerGameUI(gameUI);

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
                }
            );
            game.itemSystem.itemSpawned += OnDroppedItemSpawned;
            game.itemSystem.itemDeleted += OnDroppedItemDeleted;
            game.projectileSpawned += OnProjectileSpawned;
            game.projectileDeleted += OnProjectileDeleted;
            game.agentSpawned += OnAgentSpawned;

            this.heroParty = new GameCore.AgentsParty(
                game: game,
                agentPartyData: new DataDefinition.AgentParty("good party")
            );
            this.monsterParty = new GameCore.AgentsParty(
                game: game,
                agentPartyData: new DataDefinition.AgentParty("evil party")
            );

            var sceneAgents = FindObjectsOfType<SceneAgent>();
            foreach (var sceneAgent in sceneAgents)
            {
                // // if (agent == controlledAgent) {
                // //     agent.Setup(this, agentsManager.Parties[0]);
                // //     // @todo update controlledAgent health from saveData
                // // }
                // // else {
                // //     agent.Setup(this, agentsManager.Parties[1]);
                // // }

                // // agentsManager.RegisterAgent(agent);

                // var (agent, agentObj, agentCtrl) = SetupAgent(game, sceneAgent);

                // agents.Add(agent);
                // agentObjectsControllers.Add(agentObj, agentCtrl);
                var agent = CreateAgent(game, sceneAgent.AgentType);
                game.SpawnAgent(agent, sceneAgent.transform.position, Quaternion.identity);

                if (controlledAgentObject == sceneAgent.gameObject)
                {
                    controlledAgent = dict_agent_agentCtrl[agent];
                    gameplayManagerGameUI.SetControlledAgent(controlledAgent);
                }

                Destroy(sceneAgent.gameObject);
            }

            playerController = GetComponent<PlayerController>();
            playerController.Setup(this);

            playerController.MouseHover.agentMouseEntered += gameplayManagerGameUI.OnPlayerAgentMouseEntered;
            playerController.MouseHover.agentMouseLeft += gameplayManagerGameUI.OnPlayerAgentMouseLeft;

            var cameraFollow = GetComponent<CameraFollow>();
            cameraFollow.Setup(this, controlledAgent.transform);

            var ctrlAgentHealth = controlledAgent.Agent.health;

            gameUI.SetPlayerHealth(ctrlAgentHealth.CurrentPoints, ctrlAgentHealth.MaxPoints);
            gameUI.HideEnemyHealth();

            ctrlAgentHealth.healthChanged += gameplayManagerGameUI.OnControlledAgentHealthPointsChanged;

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

        // (GameCore.Agent, GameObject, AgentController) SetupAgent(GameCore.Game game, AgentController agentMB)
        // {
        //     DataDefinition.Agent agentData = null;
        //     if (agentMB.AgentType == AgentType.Hero)
        //         agentData = DataInstance.Agents.hero;
        //     else if (agentMB.AgentType == AgentType.Demon)
        //         agentData = DataInstance.Agents.demon;

        //     var agentMovement = new GameCore.AgentMovement();
        //     var agent = new GameCore.Agent(
        //         health: new GameCore.AgentHealth(),
        //         movement: agentMovement,
        //         combat: new GameCore.AgentCombat(
        //             game: game,
        //             movement: agentMovement
        //         ),
        //         agentData: agentData
        //     );

        //     agentMB.Setup(
        //         gameplayManager: this,
        //         agent: agent
        //     );

        //     var agentPrefab = agentPrefabs.Find(p => p.AgentType == agentMB.AgentType);
        //     if (agentPrefab == null) throw new System.Exception($"agent prefab not found: {agentMB.AgentType}");

        //     var agentObj = Instantiate(agentPrefab.Prefab, agentMB.transform);
        //     var agentCtrl = agentObj.GetComponent<AgentController>();

        //     var spriteModel = agentObj.GetComponentInChildren<SpriteModel>();
        //     if (spriteModel == null) throw new System.Exception($"agent SpriteModel not found: {agentMB.AgentType}");

        //     // @todo magic number
        //     spriteModel.transform.rotation = Quaternion.Euler(37.5f, 45f, 0f);

        //     // @todo reassign values before removing collider and navMeshAgent

        //     var modelCollider = agentObj.GetComponent<Collider>();
        //     Destroy(modelCollider);

        //     var modelNavMeshAgent = agentObj.GetComponent<NavMeshAgent>();
        //     Destroy(modelNavMeshAgent);

        //     Destroy(spriteModel);

        //     return (agent, agentObj, agentCtrl);
        // }

        GameCore.Agent CreateAgent(GameCore.Game game, AgentType agentType)
        {
            var dict_agentType_agentData = new Dictionary<AgentType, DataDefinition.Agent>(){
                [AgentType.Hero] = DataInstance.Agents.hero,
                [AgentType.Demon] = DataInstance.Agents.demon
            };
            var agentData = dict_agentType_agentData[agentType];

            GameCore.AgentsParty agentsParty = null;
            GameCore.AgentControl agentControl = null;
            if (agentType == AgentType.Hero)
            {
                agentsParty = heroParty;
                agentControl = new GameCore.AgentPlayerControl();
            }
            else
            {
                agentsParty = monsterParty;
                agentControl = new GameCore.AgentAIControl();
            }

            var agent = game.CreateAgent(
                agentData: agentData,
                agentsParty: agentsParty,
                agentControl: agentControl
            );

            return agent;
        }

        (GameObject, AgentController) InstantiateAgent(
            // GameCore.Game game,
            GameCore.Agent agent,
            AgentType agentType,
            Vector3 pos,
            Quaternion rot
        )
        {
            var agentObj = Instantiate(agentPrefab, pos, rot, agentsContainer);
            var agentCtrl = agentObj.GetComponent<AgentController>();

            agentCtrl.Setup(
                gameplayManager: this,
                agent: agent
            );

            var agentModelPrefab = agentModelPrefabs.Find(p => p.AgentType == agentType);
            if (agentModelPrefab == null) throw new System.Exception($"agent prefab not found: {agentType}");

            var agentModelObj = Instantiate(agentModelPrefab.Prefab, agentCtrl.transform);

            var spriteModel = agentModelObj.GetComponentInChildren<SpriteModel>();
            if (spriteModel == null) throw new System.Exception($"agent SpriteModel not found: {agentType}");

            // @todo magic number
            spriteModel.transform.rotation = Quaternion.Euler(37.5f, 45f, 0f);

            // @todo reassign values before removing collider and navMeshAgent

            var modelCollider = agentModelObj.GetComponent<Collider>();
            Destroy(modelCollider);

            var modelNavMeshAgent = agentModelObj.GetComponent<NavMeshAgent>();
            Destroy(modelNavMeshAgent);

            Destroy(spriteModel);

            return (agentObj, agentCtrl);
        }

        void OnAgentSpawned(GameCore.Agent agent, Vector3 pos, Quaternion rot)
        {
            AgentType agentType = AgentType.Hero;
            if (agent.agentData == DataInstance.Agents.hero)
                agentType = AgentType.Hero;
            else if (agent.agentData == DataInstance.Agents.demon)
                agentType = AgentType.Demon;

            // var obj = Instantiate(AgentPrefab, pos, rot, agentsContainer);
            // var projectile = obj.GetComponent<Projectile>();
            // projectile.Setup(combat.AgentController);
            var (agentObj, agentCtrl) = InstantiateAgent(agent, agentType, pos, rot);

            //     agents.Add(agent);
            //     agentObjectsControllers.Add(agentObj, agentCtrl);

            // dict_object_transformScript.Add(agentObj, agent);
            dict_transformScript_object.Add(agent, agentObj);
            dict_object_agentCtrl.Add(agentObj, agentCtrl);
            dict_agent_agentCtrl.Add(agent, agentCtrl);

            agent.Setup();
        }

        void OnProjectileSpawned(GameCore.Projectile projectile, Vector3 pos, Quaternion rot)
        {
            var projectileObject = Instantiate(ProjectilePrefab, pos, rot, ProjectilesContainer);
            var projectileMB = projectileObject.GetComponent<Projectile>();
            projectileMB.Setup(
                gameplayManager: this,
                // originatorAgentCtrl: combat.AgentController,
                projectile: projectile
            );

            // dict_object_transformScript.Add(projectileObject, projectile);
            dict_transformScript_object.Add(projectile, projectileObject);
        }

        void OnProjectileDeleted(GameCore.Projectile projectile)
        {
            var projectileObject = dict_transformScript_object[projectile];

            Destroy(projectileObject);
        }

        void OnDroppedItemSpawned(GameCore.DroppedItem droppedItem, Vector3 pos)
        {
            var itemTypes = new Dictionary<DataDefinition.Item, ItemType>(){
                [DataInstance.Items.shortSword] = ItemType.Sword,
                [DataInstance.Items.handAxe] = ItemType.Axe,
                [DataInstance.Items.shortStaff] = ItemType.Staff,
                [DataInstance.Items.skullCap] = ItemType.Helm,
                [DataInstance.Items.smallShield] = ItemType.Shield,
            };
            var itemType = itemTypes[droppedItem.item.itemData];
            var rot = Quaternion.identity;
            var (droppedItemObject, droppedItemMB) = InstantiateDroppedItem(droppedItem, itemType, pos, rot);

            dict_transformScript_object.Add(droppedItem, droppedItemObject);
        }

        (GameObject, DroppedItemMB) InstantiateDroppedItem(
            GameCore.DroppedItem droppedItem,
            ItemType itemType,
            Vector3 pos,
            Quaternion rot
        )
        {
            var droppedItemObject = Instantiate(DroppedItemPrefab, pos, rot, ProjectilesContainer);
            var droppedItemMB = droppedItemObject.GetComponent<DroppedItemMB>();
            droppedItemMB.Setup(
                gameplayManager: this,
                droppedItem: droppedItem
            );

            var droppedItemModelPrefab = itemModelPrefabs.Find(p => p.ItemType == itemType);
            if (droppedItemModelPrefab == null) throw new System.Exception($"droppedItem prefab not found: {itemType}");

            var droppedItemModelObj = Instantiate(droppedItemModelPrefab.Prefab, droppedItemMB.transform);

            return (droppedItemObject, droppedItemMB);
        }

        void OnDroppedItemDeleted(GameCore.DroppedItem droppedItem)
        {
            var droppedItemObject = dict_transformScript_object[droppedItem];

            Destroy(droppedItemObject);
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
