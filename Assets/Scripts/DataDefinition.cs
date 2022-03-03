namespace DataDefinition
{
    public class AgentParty
    {
        private string name;
        public string Name => name;

        public AgentParty(
            string name
        )
        {
            this.name = name;
        }
    }

    public class Agent
    {
        private string name;
        public string Name => name;

        public Agent(
            string name
        )
        {
            this.name = name;
        }
    }

    public class QuestTask
    {

    }

    public class Quest
    {

    }

    // weapon (sword, axe, staff), helmet, shield
    public class ItemCategory
    {
        public readonly string name;
        public readonly ItemCategory parentCategory = null;

        public ItemCategory(string name, ItemCategory parentCategory = null)
        {
            this.name = name;
            this.parentCategory = parentCategory;
        }
    }

    public class Item
    {
        public readonly string name;
        public readonly ItemCategory category;

        public Item(string name, ItemCategory category)
        {
            this.name = name;
            this.category = category;
        }
    }
}