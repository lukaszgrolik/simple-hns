using System.Collections;
using System.Collections.Generic;

namespace MonoBehaviors
{
    public enum SkillType
    {
        DefaultMelee,
        FireBolt,
        FireBall,
        EnergyBolt,
        TreeWall,
        SummonButterflies,
        SkeletonFireBolt,
        HoodedWarriorMageEnergyBolt,
    }

    public class DataStore
    {
        public readonly Dictionary<SkillType, DataDefinition.Skill> skills = new Dictionary<SkillType, DataDefinition.Skill>()
        {
            [SkillType.DefaultMelee] = new DataDefinition.Skill_Melee(
                name: "Default melee"
            ),
            [SkillType.FireBolt] = new DataDefinition.Skill_CastProjectile(
                name: "Fire bolt",
                speed: 15,
                damage: 20,
                damageDeviation: .25f
            ),
            [SkillType.SkeletonFireBolt] = new DataDefinition.Skill_CastProjectile(
                name: "Skeleton Fire bolt",
                speed: 15,
                damage: 5,
                damageDeviation: .25f
            ),
            [SkillType.FireBall] = new DataDefinition.Skill_CastProjectile(
                name: "Fire ball",
                speed: 20,
                damage: 32,
                damageDeviation: .25f,
                splashRadius: 2
            ),
            [SkillType.EnergyBolt] = new DataDefinition.Skill_CastProjectile(
                name: "Energy bolt",
                speed: 25,
                damage: 20,
                damageDeviation: .25f
            ),
            [SkillType.HoodedWarriorMageEnergyBolt] = new DataDefinition.Skill_CastProjectile(
                name: "Hooded Warrior Mage Energy bolt",
                speed: 15,
                damage: 8.5f,
                damageDeviation: .2f
            ),

            [SkillType.TreeWall] = new DataDefinition.Skill_Custom(
                name: "Tree wall"
            ),


        };

        public readonly Dictionary<ProjectileType, DataDefinition.Skill_CastProjectile> projectiles = new Dictionary<ProjectileType, DataDefinition.Skill_CastProjectile>();

        public readonly Dictionary<AgentType, DataDefinition.Agent> agents = new Dictionary<AgentType, DataDefinition.Agent>();

        public Dictionary<DataDefinition.Skill, SkillType> skillTypes = new Dictionary<DataDefinition.Skill, SkillType>();
        public Dictionary<DataDefinition.Skill_CastProjectile, ProjectileType> projectileTypes = new Dictionary<DataDefinition.Skill_CastProjectile, ProjectileType>();
        public Dictionary<DataDefinition.Agent, AgentType> agentTypes = new Dictionary<DataDefinition.Agent, AgentType>();

        public DataStore()
        {
            projectiles.Add(ProjectileType.FireBolt, skills[SkillType.FireBolt] as DataDefinition.Skill_CastProjectile);
            projectiles.Add(ProjectileType.FireBall, skills[SkillType.FireBall] as DataDefinition.Skill_CastProjectile);
            projectiles.Add(ProjectileType.EnergyBolt, skills[SkillType.EnergyBolt] as DataDefinition.Skill_CastProjectile);
            projectiles.Add(ProjectileType.SkeletonFireBolt, skills[SkillType.SkeletonFireBolt] as DataDefinition.Skill_CastProjectile);
            projectiles.Add(ProjectileType.HoodedWarriorMageEnergyBolt, skills[SkillType.HoodedWarriorMageEnergyBolt] as DataDefinition.Skill_CastProjectile);

            agents.Add(AgentType.Butterfly, new DataDefinition.Agent(
                name: "Butterfly",
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.DefaultMelee],
                    skills[SkillType.FireBolt],
                }
            ));

            skills.Add(SkillType.SummonButterflies, new DataDefinition.Skill_SummonAgent(
                name: "Summon butterflies",
                agent: agents[AgentType.Butterfly],
                amount: 3
            ));

            agents.Add(AgentType.Hero, new DataDefinition.Agent(
                name: "Hero",
                health: 150,
                // healthDeviation: .3f,
                canBeStunned: false,
                walkingSpeed: 3,
                runningSpeed: 6,
                // speedDeviation: .15f,
                // sightRadius: 5,
                // sightRadiusDeviation: .15f,
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.DefaultMelee],
                    skills[SkillType.FireBolt],
                    skills[SkillType.FireBall],
                    skills[SkillType.EnergyBolt],
                },
                attackRate: 5f
            ));

            agents.Add(AgentType.Warden, new DataDefinition.Agent(
                name: "Warden",
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.DefaultMelee],
                    skills[SkillType.FireBolt],
                }
            ));
            // agents.Add(AgentType.Smith, new DataDefinition.Agent("Smith");

            agents.Add(AgentType.Demon, new DataDefinition.Agent(
                name: "Demon",
                health: 50,
                healthDeviation: .2f,
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.DefaultMelee],
                }
            ));
            agents.Add(AgentType.Demon_Orange, new DataDefinition.Agent(
                name: "Demon",
                health: 50,
                healthDeviation: .2f,
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.DefaultMelee],
                }
            ));
            agents.Add(AgentType.Demon_Blue, new DataDefinition.Agent(
                name: "Demon",
                health: 50,
                healthDeviation: .2f,
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.DefaultMelee],
                }
            ));
            agents.Add(AgentType.Demon_Black, new DataDefinition.Agent(
                name: "Demon",
                health: 50,
                healthDeviation: .2f,
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.DefaultMelee],
                }
            ));

            agents.Add(AgentType.Warrior, new DataDefinition.Agent(
                name: "Possessed Warrior",
                health: 200,
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.DefaultMelee],
                }
            ));

            agents.Add(AgentType.HoodedWarrior, new DataDefinition.Agent(
                name: "Hooded Warrior",
                health: 100,
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.DefaultMelee],
                    // skills[SkillType.FireBolt],
                }
            ));
            agents.Add(AgentType.HoodedWarriorMage, new DataDefinition.Agent(
                name: "Hooded Warrior Mage",
                health: 80,
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.HoodedWarriorMageEnergyBolt],
                }
            ));

            agents.Add(AgentType.Zombie, new DataDefinition.Agent(
                name: "Zombie",
                health: 10,
                healthDeviation: .3f,
                walkingSpeed: 3,
                runningSpeed: 5,
                speedDeviation: .15f,
                sightRadius: 5,
                sightRadiusDeviation: .15f,
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.DefaultMelee],
                }
            ));
            agents.Add(AgentType.Zombie_Desert, new DataDefinition.Agent(
                name: "Zombie",
                health: 10,
                healthDeviation: .3f,
                walkingSpeed: 3,
                runningSpeed: 5,
                speedDeviation: .15f,
                sightRadius: 5,
                sightRadiusDeviation: .15f,
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.DefaultMelee],
                }
            ));
            agents.Add(AgentType.Zombie_Snow, new DataDefinition.Agent(
                name: "Zombie",
                health: 10,
                healthDeviation: .3f,
                walkingSpeed: 3,
                runningSpeed: 5,
                speedDeviation: .15f,
                sightRadius: 5,
                sightRadiusDeviation: .15f,
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.DefaultMelee],
                }
            ));

            agents.Add(AgentType.Skeleton, new DataDefinition.Agent(
                name: "Skeleton",
                health: 50,
                healthDeviation: .2f,
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.DefaultMelee],
                }
            ));
            agents.Add(AgentType.SkeletonArcher, new DataDefinition.Agent(
                name: "Skeleton Archer",
                health: 50,
                healthDeviation: .2f,
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.DefaultMelee],
                    skills[SkillType.SkeletonFireBolt],
                }
            ));
            agents.Add(AgentType.SkeletonMage, new DataDefinition.Agent(
                name: "Skeleton Mage",
                health: 50,
                healthDeviation: .2f,
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.SkeletonFireBolt],
                }
            ));
            agents.Add(AgentType.Bulbfrog, new DataDefinition.Agent("Bulbfrog"));
            agents.Add(AgentType.Ent, new DataDefinition.Agent(
                name: "Ent",
                skills: new List<DataDefinition.Skill>()
                {
                    skills[SkillType.DefaultMelee],
                    skills[SkillType.EnergyBolt],
                    skills[SkillType.TreeWall],
                    skills[SkillType.SummonButterflies],
                }
            ));

            agents.Add(AgentType.Crab, new DataDefinition.Agent("Crab"));
            agents.Add(AgentType.WalkingShroom, new DataDefinition.Agent("Walking Shroom"));

            //
            //
            //

            foreach (var item in skills)
            {
                skillTypes.Add(item.Value, item.Key);
            }

            foreach (var item in projectiles)
            {
                projectileTypes.Add(item.Value, item.Key);
            }

            foreach (var item in agents)
            {
                agentTypes.Add(item.Value, item.Key);
            }

        }
    }
}

namespace DataInstance
{

    // class Skills
    // {
    //     public static readonly DataDefinition.Skill_Melee DefaultMelee = new DataDefinition.Skill_Melee(
    //         name: "Default melee"
    //     );

    //     public static readonly DataDefinition.Skill_CastProjectile FireBolt = new DataDefinition.Skill_CastProjectile(
    //         name: "Fire bolt",
    //         speed: 8,
    //         damage: 20,
    //         damageDeviation: .25f
    //     );
    //     public static readonly DataDefinition.Skill_CastProjectile FireBall = new DataDefinition.Skill_CastProjectile(
    //         name: "Fire ball",
    //         speed: 11,
    //         damage: 32,
    //         damageDeviation: .25f,
    //         splashRadius: 2
    //     );
    //     public static readonly DataDefinition.Skill_CastProjectile EnergyBolt = new DataDefinition.Skill_CastProjectile(
    //         name: "Energy bolt",
    //         speed: 8,
    //         damage: 20,
    //         damageDeviation: .25f
    //     );

    //     public static readonly DataDefinition.Skill_Custom TreeWall = new DataDefinition.Skill_Custom(
    //         name: "Tree wall"
    //     );

    //     public static readonly DataDefinition.Skill_SummonAgent SummonButterflies = new DataDefinition.Skill_SummonAgent(
    //         name: "Summon butterflies",
    //         agent: Agents.Butterfly,
    //         amount: 3
    //     );
    // }

    // class Agents
    // {

    // }

    class ItemCategories
    {
        public static readonly DataDefinition.ItemCategory weapon = new DataDefinition.ItemCategory("Weapon");
        public static readonly DataDefinition.ItemCategory sword = new DataDefinition.ItemCategory("Sword", ItemCategories.weapon);
        public static readonly DataDefinition.ItemCategory axe = new DataDefinition.ItemCategory("Axe", ItemCategories.weapon);
        public static readonly DataDefinition.ItemCategory staff = new DataDefinition.ItemCategory("Staff", ItemCategories.weapon);

        public static readonly DataDefinition.ItemCategory armor = new DataDefinition.ItemCategory("Armor");
        public static readonly DataDefinition.ItemCategory helm = new DataDefinition.ItemCategory("Helm", ItemCategories.armor);
        public static readonly DataDefinition.ItemCategory shield = new DataDefinition.ItemCategory("Shield", ItemCategories.armor);
    }

    class Items
    {
        public static readonly DataDefinition.Item shortSword = new DataDefinition.Item("Short Sword", ItemCategories.sword);
        public static readonly DataDefinition.Item handAxe = new DataDefinition.Item("Hand Axe", ItemCategories.axe);
        public static readonly DataDefinition.Item shortStaff = new DataDefinition.Item("Short Staff", ItemCategories.staff);

        public static readonly DataDefinition.Item skullCap = new DataDefinition.Item("Skull Cap", ItemCategories.helm);
        public static readonly DataDefinition.Item smallShield = new DataDefinition.Item("Small Shield", ItemCategories.shield);
    }
}