using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public interface ITransformScript
    {
        Vector3 GetPosition();
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

        public readonly GameAgentLeveling agentLeveling = new GameAgentLeveling();

        private readonly List<AgentsGroup> agentsGroups = new List<AgentsGroup>();

        private readonly List<Agent> agents = new List<Agent>();
        private readonly List<Projectile> projectiles = new List<Projectile>();

        public readonly LocationSystem locationSystem;
        public readonly ItemSystem itemSystem;
        public readonly QuestSystem questSystem;

        private List<AgentsParty> agentsParties;
        public List<AgentsParty> AgentsParties => agentsParties;
        private List<AgentsParty> enemyParties;
        public List<AgentsParty> EnemyParties => enemyParties;

        private Agent playerAgent;
        public Agent PlayerAgent => playerAgent;

        public event System.Action<Projectile, Vector3, Quaternion> projectileSpawned;
        public event System.Action<Projectile> projectileDeleted;

        public event System.Action<Agent, Vector3, Quaternion> agentSpawned;

        public event System.Action<Location> playerEnteredLocation;

        public Game()
        {
            this.locationSystem = new LocationSystem();
            this.itemSystem = new ItemSystem(game: this);
            this.questSystem = new QuestSystem();

            locationSystem.locationEnteredByAgent += OnLocationEnteredByAgent;
        }

        void OnLocationEnteredByAgent(Location loc, Agent agent)
        {
            if (agent == playerAgent)
            {
                playerEnteredLocation?.Invoke(loc);
            }
        }

        public void InitAgentsParties(
            List<AgentsParty> agentsParties,
            List<AgentsParty> enemyParties
        )
        {
            this.agentsParties = agentsParties;

            for (int i = 0; i < agentsParties.Count; i++)
            {
                agentsParties[i].Setup(this);
            }

            this.enemyParties = enemyParties;
        }

        public void SetPlayerAgent(Agent agent)
        {
            this.playerAgent = agent;
        }

        public void SetLocations(List<Location> locations)
        {
            this.locationSystem.SetLocations(locations);
        }

        public void SetQuests(List<Quest> quests)
        {
            this.questSystem.SetQuests(quests);
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

        public AgentsGroup CreateAgentsGroup()
        {
            return new AgentsGroup(this);
        }

        public Agent CreateAgent(
            DataDefinition.Agent agentData,
            AgentsGroup agentsGroup,
            AgentsParty agentsParty,
            AgentControl agentControl
        )
        {
            var agentLevel = new AgentLevel();
            var agentCard = new AgentCard();
            var agentEquipment = new AgentEquipment();
            var agentHealth = new AgentHealth(
                maxPoints: Utils.RandomValueWithDeviation(agentData.health, agentData.healthDeviation)
            );
            var agentStun = new AgentStun(
                agentHealth: agentHealth,
                canBeStunned: agentData.canBeStunned,
                stunTime: agentData.stunTime
            );
            var agentMovement = new AgentMovement(
                agentHealth: agentHealth,
                agentStun: agentStun,
                walkingSpeed: Utils.RandomValueWithDeviation(agentData.walkingSpeed, agentData.speedDeviation),
                runningSpeed: Utils.RandomValueWithDeviation(agentData.runningSpeed, agentData.speedDeviation)
            );

            var combat = new AgentCombat(
                game: this,
                agentStun: agentStun,
                agentMovement: agentMovement,
                attackRate: agentData.attackRate
            );
            var combatSkills = new List<Skill>();

            // Debug.Log($"agentData: {agentData.name} | agentData.skills.Count: {agentData.skills.Count}");
            for (int i = 0; i < agentData.skills.Count; i++)
            {
                var skill = agentData.skills[i];

                // Debug.Log($"skill: {skill}");
                // Debug.Log($"skill.name: {skill.name}");
                // Debug.Log($"skill.GetType(): {skill.GetType()}");
                // Debug.Log($"typeof(DataDefinition.Skill_Melee): {typeof(DataDefinition.Skill_Melee)}");
                if (skill.GetType() == typeof(DataDefinition.Skill_Melee))
                    combatSkills.Add(new MeleeAttackSkill(combat, skill as DataDefinition.Skill_Melee));
                else if (skill.GetType() == typeof(DataDefinition.Skill_Custom))
                    combatSkills.Add(new CustomSkill(combat, skill as DataDefinition.Skill_Custom));
                else if (skill.GetType() == typeof(DataDefinition.Skill_Bow))
                    combatSkills.Add(new BowSkill(combat, skill as DataDefinition.Skill_Bow));
                else if (skill.GetType() == typeof(DataDefinition.Skill_CastProjectile))
                    combatSkills.Add(new ProjectileSkill(combat, skill as DataDefinition.Skill_CastProjectile));
                else if (skill.GetType() == typeof(DataDefinition.Skill_SummonAgent))
                    combatSkills.Add(new SummonSkill(combat, skill as DataDefinition.Skill_SummonAgent));
            }

            var agent = new Agent(
                game: this,
                groupMember: new AgentGroupMember(
                    agentsGroup: agentsGroup
                ),
                agentLevel: agentLevel,
                agentCard: agentCard,
                equipment: agentEquipment,
                agentCardEquipment: new AgentCardEquipment(
                    agentCard: agentCard,
                    agentEquipment: agentEquipment
                ),
                health: agentHealth,
                stun: agentStun,
                drop: new AgentDrop(
                    health: agentHealth
                ),
                movement: agentMovement,
                partyMember: new AgentPartyMember(
                    game: this,
                    agentsParty: agentsParty
                ),
                agentDetection: new AgentDetection(
                    sightRadius: Utils.RandomValueWithDeviation(agentData.sightRadius, agentData.sightRadiusDeviation)
                ),
                combat: combat,
                agentHealthCombat: new AgentHealthCombat(
                    agentHealth: agentHealth,
                    agentCombat: combat
                ),
                agentCombatLevel: new AgentCombatLevel(
                    agentCombat: combat,
                    agentLevel: agentLevel
                ),
                control: agentControl,
                agentData: agentData
            );

            combat.SetSkills(combatSkills);

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