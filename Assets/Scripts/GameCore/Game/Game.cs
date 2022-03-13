using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public interface ITransformScript
    {

    }

    // public interface IGameGetPosition
    // {
    //     Vector3 GetPosition(ITransformScript script);
    // }
    // public interface IGetProjectileSpawnPosition
    // {
    //     Vector3 GetProjectileSpawnPosition(ITransformScript script);
    // }

    // public interface IGameSpawnProjectile
    // {
    //     void SpawnProjectile(Projectile projectile, Vector3 pos, Quaternion rot);
    // }

    public abstract class Game
    {
        public readonly EngineTime engineTime = new EngineTime();

        private readonly List<Agent> agents = new List<Agent>();
        private readonly List<Projectile> projectiles = new List<Projectile>();

        public readonly ItemSystem itemSystem;
        public readonly QuestSystem questSystem;

        private List<GameCore.AgentsParty> agentsParties;
        public List<GameCore.AgentsParty> AgentsParties => agentsParties;
        private List<GameCore.AgentsParty> enemyParties;
        public List<GameCore.AgentsParty> EnemyParties => enemyParties;

        public event System.Action<Projectile, Vector3, Quaternion> projectileSpawned;
        public event System.Action<Projectile> projectileDeleted;

        public event System.Action<Agent, Vector3, Quaternion> agentSpawned;

        public Game(
            List<Quest> quests
        )
        {
            this.itemSystem = new ItemSystem();
            this.questSystem = new QuestSystem(
                quests: quests
            );
        }

        public void InitAgentsParties(
            List<GameCore.AgentsParty> agentsParties,
            List<GameCore.AgentsParty> enemyParties
        )
        {
            this.agentsParties = agentsParties;

            for (int i = 0; i < agentsParties.Count; i++)
            {
                agentsParties[i].Setup(this);
            }

            this.enemyParties = enemyParties;
        }

        abstract public Vector3 GetPosition(ITransformScript script);
        abstract public Vector3 GetProjectileSpawnPosition(Agent agent);
        abstract public List<Agent> FindAgentsInRadius(Vector3 pos, float radius);

        public void OnUpdate(float deltaTime)
        {
            engineTime.AddDeltaTime(deltaTime);

            for (int i = 0; i < agents.Count; i++)
                agents[i].OnUpdate(deltaTime);

            for (int i = 0; i < projectiles.Count; i++)
                projectiles[i].OnUpdate(deltaTime);
        }

        public GameCore.Agent CreateAgent(
            DataDefinition.Agent agentData,
            GameCore.AgentsParty agentsParty,
            GameCore.AgentControl agentControl
        )
        {
            var agentHealth = new GameCore.AgentHealth();
            var agentMovement = new GameCore.AgentMovement(agentHealth);
            var agent = new GameCore.Agent(
                game: this,
                equipment: new GameCore.AgentEquipment(),
                health: agentHealth,
                drop: new GameCore.AgentDrop(
                    health: agentHealth
                ),
                movement: agentMovement,
                partyMember: new GameCore.AgentPartyMember(
                    game: this,
                    agentsParty: agentsParty
                ),
                agentDetection: new GameCore.AgentDetection(),
                combat: new GameCore.AgentCombat(
                    game: this,
                    movement: agentMovement
                ),
                control: agentControl,
                agentData: agentData
            );

            return agent;
        }

        public void SpawnAgent(Agent agent, Vector3 pos, Quaternion rot)
        {
            agents.Add(agent);

            agentSpawned?.Invoke(agent, pos, rot);
        }

        public void SpawnProjectile(Projectile projectile, Vector3 pos, Quaternion rot)
        {
            projectiles.Add(projectile);

            projectileSpawned?.Invoke(projectile, pos, rot);
        }

        public void DeleteProjectile(Projectile projectile)
        {
            projectiles.Remove(projectile);

            projectileDeleted?.Invoke(projectile);
        }

        // public void SetEnemyParties()
        // {
        //     this.enemyParties = agentsParties;
        // }
    }
}