using System.Collections;
using System.Collections.Generic;

namespace DataDefinition
{
//     public abstract class Skill
//     {
//         public readonly string name;

//         public Skill(string name)
//         {
//             this.name = name;
//         }
//     }

//     public class Skill_Custom : Skill
//     {
//         public Skill_Custom(
//             string name
//         ) : base(name)
//         {

//         }
//     }
//     public class Skill_Melee : Skill
//     {
//         public readonly float damage;
//         public readonly float damageDeviation;
//         public readonly float angle;

//         public Skill_Melee(
//             string name,
//             float damage = 10,
//             float damageDeviation = 0,
//             float angle = 0
//         ) : base(name)
//         {
//             this.damage = damage;
//             this.damageDeviation = damageDeviation;
//             this.angle = angle;
//         }
//     }

//     public class Skill_Bow : Skill
//     {
//         public readonly float damage = 10;
//         public readonly float damageDeviation = 0;

//         public Skill_Bow(
//             string name,
//             float damage = 10,
//             float damageDeviation = 0
//         ) : base(name)
//         {
//             this.damage = damage;
//             this.damageDeviation = damageDeviation;
//         }
//     }

//     public class Skill_CastProjectile : Skill
//     {
//         public readonly float speed;
//         public readonly float damage;
//         public readonly float damageDeviation;
//         public readonly float splashRadius;

//         public Skill_CastProjectile(
//             string name,
//             float speed = 20,
//             float damage = 10,
//             float damageDeviation = 0,
//             float splashRadius = 0
//         ) : base(name)
//         {
//             this.speed = speed;
//             this.damage = damage;
//             this.damageDeviation = damageDeviation;
//             this.splashRadius = splashRadius;
//         }
//     }

//     public class Skill_SummonAgent : Skill
//     {
//         public readonly Agent agent;
//         public readonly int amount;

//         public Skill_SummonAgent(
//             string name,
//             Agent agent,
//             int amount = 1
//         ) : base(name)
//         {
//             this.agent = agent;
//             this.amount = amount;
//         }
//     }

//     public class AgentParty
//     {
//         public readonly string name;

//         public AgentParty(
//             string name
//         )
//         {
//             this.name = name;
//         }
//     }

//     public class Agent
//     {
//         public readonly string name;

//         public readonly float health;
//         public readonly float healthDeviation;

//         public readonly bool canBeStunned;
//         public readonly float stunTime;

//         public readonly float walkingSpeed;
//         public readonly float runningSpeed;
//         public readonly float speedDeviation;

//         public readonly float sightRadius;
//         public readonly float sightRadiusDeviation;

//         public readonly List<Skill> skills;
//         public readonly float attackRate;

//         public Agent(
//             string name,

//             float health = 10f,
//             float healthDeviation = 0,

//             bool canBeStunned = true,
//             float stunTime = 1f,

//             float walkingSpeed = 3f,
//             float runningSpeed = 6f,
//             float speedDeviation = 0f,

//             float sightRadius = 10f,
//             float sightRadiusDeviation = 0f,

//             List<Skill> skills = null,
//             float attackRate = 1f
//         )
//         {
//             this.name = name;

//             this.health = health;
//             this.healthDeviation = healthDeviation;

//             this.canBeStunned = canBeStunned;
//             this.stunTime = stunTime;

//             this.walkingSpeed = walkingSpeed;
//             this.runningSpeed = runningSpeed;
//             this.speedDeviation = speedDeviation;

//             this.sightRadius = sightRadius;
//             this.sightRadiusDeviation = sightRadiusDeviation;

//             this.skills = skills != null ? skills : new List<Skill>();
//             this.attackRate = attackRate;
//         }
//     }

//     // weapon (sword, axe, staff), helmet, shield
//     public class ItemCategory
//     {
//         public readonly string name;
//         public readonly ItemCategory parentCategory = null;

//         public ItemCategory(string name, ItemCategory parentCategory = null)
//         {
//             this.name = name;
//             this.parentCategory = parentCategory;
//         }
//     }

//     public class Item
//     {
//         public readonly string name;
//         public readonly ItemCategory category;

//         public Item(string name, ItemCategory category)
//         {
//             this.name = name;
//             this.category = category;
//         }
//     }

//     public class Location
//     {
//         private string name;

//         // public readonly entrances

//         public Location(string name)
//         {
//             this.name = name;
//         }
//     }

//     public class QuestTask
//     {

//     }

//     public class QuestTask_KillAllInLocation : QuestTask {
//         public QuestTask_KillAllInLocation(Location location)
//         {

//         }
//     }

//     public class QuestTask_EnterLocation : QuestTask {
//         public QuestTask_EnterLocation(Location location)
//         {

//         }
//     }

//     public class QuestTask_EnterProximity : QuestTask {
//         public QuestTask_EnterProximity(Agent agent)
//         {

//         }
//     }

//     public class QuestTask_CollectItem : QuestTask {
//         public QuestTask_CollectItem(Item item)
//         {

//         }
//     }

//     public class QuestTask_DeliverItem : QuestTask {}

//     public class QuestTask_MeetAgent : QuestTask {}

//     public class Quest
//     {
//         public readonly string title;
//         public readonly string description;
//         public readonly List<QuestTask> tasks;

//         public Quest(
//             string title,
//             string description,
//             List<QuestTask> tasks
//         )
//         {
//             this.title = title;
//             this.description = description;
//             this.tasks = tasks;
//         }
//     }

    // public class SpecialAgent
    // {
    //     public readonly string name;

    //     public SpecialAgent(string name)
    //     {
    //         this.name = name;
    //     }
    // }

    // public class SpecialItem
    // {
    //     public readonly string name;

    //     public SpecialItem(string name)
    //     {
    //         this.name = name;
    //     }
    // }
}