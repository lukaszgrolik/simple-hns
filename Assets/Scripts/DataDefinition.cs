namespace DataDefinition
{
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