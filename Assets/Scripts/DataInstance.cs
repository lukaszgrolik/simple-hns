using System.Collections;
using System.Collections.Generic;

namespace DataInstance
{
    class Agents
    {
        public static readonly DataDefinition.Agent hero = new DataDefinition.Agent("Hero");
        public static readonly DataDefinition.Agent warden = new DataDefinition.Agent("Warden");
        public static readonly DataDefinition.Agent smith = new DataDefinition.Agent("Smith");

        public static readonly DataDefinition.Agent demon = new DataDefinition.Agent("Demon");
        public static readonly DataDefinition.Agent zombie = new DataDefinition.Agent("Zombie");
        public static readonly DataDefinition.Agent skeleton = new DataDefinition.Agent("Skeleton");
        public static readonly DataDefinition.Agent skeletonArcher = new DataDefinition.Agent("Skeleton Archer");
        public static readonly DataDefinition.Agent skeletonMage = new DataDefinition.Agent("Skeleton Mage");
    }

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