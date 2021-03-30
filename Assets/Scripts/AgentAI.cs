using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class StateMachine {
    private State state;

    public void SetState(State nextState) {
        if (state != null) state.Exit();

        state = nextState;
        nextState.Enter();
    }

    public virtual void Exit() {
        if (state != null) state.Exit();
    }
}

abstract class State {
    protected StateMachine stateMachine;

    public State(StateMachine stateMachine) {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() {

    }

    public virtual void Exit() {

    }
}

// class AIStateMachine : StateMachine {
//     private AgentController agent;

//     public AIStateMachine(AgentController agent) {
//         this.agent = agent;
//     }
// }

namespace AIStates {
    class Patrol : State {
        private StateMachine patrolSM = new StateMachine();

        private AgentController agent;

        public Patrol(StateMachine stateMachine, AgentController agent) : base(stateMachine) {
            this.agent = agent;
        }

        public override void Enter() {
            patrolSM.SetState(new PatrolStates.Moving(stateMachine, agent));
        }

        public override void Exit() {
            patrolSM.Exit();
        }
    }

    class Engage : State {
        private AgentController agent;

        public Engage(StateMachine stateMachine, AgentController agent) : base(stateMachine) {
            this.agent = agent;
        }

        public override void Enter() {
            var enemy = agent.VisibleEnemies.Random();
            if (enemy) {
                stateMachine.SetState(new Combat(stateMachine, agent, enemy));
            }
        }
    }

    class Combat : State {
        AgentController agent;
        AgentController enemy;

        IEnumerator keepAttackingCoroutine;

        public Combat(StateMachine stateMachine, AgentController agent, AgentController enemy) : base(stateMachine) {
            this.agent = agent;
            this.enemy = enemy;
        }

        public override void Enter() {
            keepAttackingCoroutine = KeepAttacking();
            agent.StartCoroutine(keepAttackingCoroutine);
        }

        public override void Exit() {
            if (keepAttackingCoroutine != null) agent.StopCoroutine(keepAttackingCoroutine);
        }

        IEnumerator KeepAttacking() {
            while (true) {
                yield return new WaitForSeconds(.5f);

                agent.Combat.Attack(enemy);
            }
        }
    }
}

namespace AIStates.PatrolStates {
    class Waiting : State {
        private AgentController agent;

        private IEnumerator waitCoroutine;

        public Waiting(StateMachine stateMachine, AgentController agent): base(stateMachine) {
            this.agent = agent;
        }

        public override void Enter() {
            waitCoroutine = Wait();
            agent.StartCoroutine(waitCoroutine);
        }

        public override void Exit() {
            if (waitCoroutine != null) agent.StopCoroutine(waitCoroutine);
        }

        private IEnumerator Wait() {
            yield return new WaitForSeconds(1);

            stateMachine.SetState(new PatrolStates.Moving(stateMachine, agent));
        }
    }

    class Moving : State {
        private AgentController agent;

        private float radius = 5f;

        public Moving(StateMachine stateMachine, AgentController agent): base(stateMachine) {
            this.agent = agent;
        }

        public override void Enter() {
            agent.Movement.Arrived.AddListener(OnAgentArrived);
            agent.Movement.SetDestination(GetRandomPos());
        }

        public override void Exit() {
            agent.Movement.Arrived.RemoveListener(OnAgentArrived);
        }

        Vector3 GetRandomPos() {
            var randomPos = Random.insideUnitCircle * radius;

            return agent.transform.position + new Vector3(randomPos.x, 0, randomPos.y);
        }

        void OnAgentArrived() {
            stateMachine.SetState(new PatrolStates.Waiting(stateMachine, agent));
        }
    }
}

public class AgentAI : MonoBehaviour {
    private AgentController agentController;

    private StateMachine sm = new StateMachine();

    public void Setup(AgentController agentController) {
        this.agentController = agentController;

        agentController.Health.Died.AddListener(OnAgentDied);
        agentController.EnemyDetected.AddListener(OnEnemyDetected);
        agentController.EnemyLost.AddListener(OnEnemyLost);

        HandleEnemiesNumberChange();
    }

    void OnEnemyDetected() {
        HandleEnemiesNumberChange();
    }

    void OnEnemyLost() {
        HandleEnemiesNumberChange();
    }

    void HandleEnemiesNumberChange() {
        if (agentController.VisibleEnemies.Count > 0) {
            sm.SetState(new AIStates.Engage(sm, agentController));
        }
        else {
            sm.SetState(new AIStates.Patrol(sm, agentController));
        }
    }

    void OnAgentDied(AgentController agent) {
        sm.Exit();

        agentController.EnemyDetected.RemoveListener(OnEnemyDetected);
        agentController.EnemyLost.RemoveListener(OnEnemyLost);
    }
}
