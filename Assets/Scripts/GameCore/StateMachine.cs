namespace GameCore.SM
{
    public class StateMachine
    {
        private State state;

        public void SetState(State nextState)
        {
            if (state != null) state.Exit();

            state = nextState;
            nextState.Enter();
        }

        public virtual void Exit()
        {
            if (state != null) state.Exit();
        }
    }

    public abstract class State
    {
        protected StateMachine stateMachine;

        public State(StateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public virtual void Enter()
        {

        }

        public virtual void Exit()
        {

        }
    }
}
