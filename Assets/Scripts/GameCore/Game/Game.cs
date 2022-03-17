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

        private readonly List<AgentsGroup> agentsGroups = new List<AgentsGroup>();

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

        public GameCore.AgentsGroup CreateAgentsGroup()
        {
            return new AgentsGroup(this);
        }

        public GameCore.Agent CreateAgent(
            DataDefinition.Agent agentData,
            AgentsGroup agentsGroup,
            GameCore.AgentsParty agentsParty,
            GameCore.AgentControl agentControl
        )
        {
            var agentHealth = new GameCore.AgentHealth(
                maxPoints: Utils.RandomValueWithDeviation(agentData.health, agentData.healthDeviation)
            );
            var agentMovement = new GameCore.AgentMovement(
                agentHealth: agentHealth,
                walkingSpeed: Utils.RandomValueWithDeviation(agentData.walkingSpeed, agentData.speedDeviation),
                runningSpeed: Utils.RandomValueWithDeviation(agentData.runningSpeed, agentData.speedDeviation)
            );

            var combat = new GameCore.AgentCombat(
                game: this,
                movement: agentMovement
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

            var agent = new GameCore.Agent(
                game: this,
                groupMember: new AgentGroupMember(
                    agentsGroup: agentsGroup
                ),
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
                agentDetection: new GameCore.AgentDetection(
                    sightRadius: Utils.RandomValueWithDeviation(agentData.sightRadius, agentData.sightRadiusDeviation)
                ),
                combat: combat,
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