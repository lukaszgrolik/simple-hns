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
}