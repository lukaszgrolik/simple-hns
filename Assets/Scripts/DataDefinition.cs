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
}