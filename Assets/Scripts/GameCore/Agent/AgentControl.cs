namespace GameCore
{
    public abstract class AgentControl : AgentComponent
    {
        public abstract void Setup();
    }

    public interface IAgentAITickableState
    {
        void OnUpdate();
    }

    public sealed class AgentAIControl : AgentControl
    {
        private SM.StateMachine sm = new SM.StateMachine();

        public override void Setup()
        {
            agent.health.died += OnAgentDied;
            agent.agentDetection.enemyDetected += OnEnemyDetected;
            agent.agentDetection.enemyLost += OnEnemyLost;

            HandleEnemiesNumberChange();
        }

        public void OnUpdate()
        {
            if (sm.State is IAgentAITickableState state)
            {
                state.OnUpdate();
            }
        }

        void OnEnemyDetected(Agent agent)
        {
            HandleEnemiesNumberChange();
        }

        void OnEnemyLost(Agent agent)
        {
            HandleEnemiesNumberChange();
        }

        void HandleEnemiesNumberChange()
        {
            if (agent.agentDetection.aliveEnemies.Count > 0)
            {
                sm.SetState(new AgentAI.States.Engage(sm, agent));
            }
            else
            {
                sm.SetState(new AgentAI.States.Patrol(sm, agent));
            }
        }

        void OnAgentDied(Agent agent)
        {
            this.agent.agentDetection.enemyDetected -= OnEnemyDetected;
            this.agent.agentDetection.enemyLost -= OnEnemyLost;

            sm.Exit();
        }
    }

    public sealed class AgentPlayerControl : AgentControl
    {
        public override void Setup()
        {

        }
    }
}