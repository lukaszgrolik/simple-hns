using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviors
{
    public class AgentController : MonoBehaviour
    {
        private GameplayManager gameplayManager;
        public GameplayManager GameplayManager => gameplayManager;

        // [SerializeField] private AgentType agentType;
        // public AgentType AgentType => agentType;

        private GameCore.Agent agent;
        public GameCore.Agent Agent => agent;

        private AgentAnimation agentAnimation;

        // private AgentMovement agentMovement;
        // public AgentMovement Movement => agentMovement;

        // private AgentHealth agentHealth;
        // public AgentHealth Health => agentHealth;

        // private Combat combat;
        // public Combat Combat => combat;

        // private AgentsDetection agentsDetection;
        // public AgentsDetection AgentsDetection => agentsDetection;

        // private AgentsManager.Party party;
        // public AgentsManager.Party Party => party;

        // public IndexedList<GameObject, AgentController> AliveEnemies => gameplayManager.AgentsManager.AliveEnemies[party];

        // private List<AgentController> visibleEnemies = new List<AgentController>();
        // public List<AgentController> VisibleEnemies => visibleEnemies;

        // public class EnemyDetectedEvent : UnityEvent { }
        // public readonly EnemyDetectedEvent enemyDetected = new EnemyDetectedEvent();

        // public class EnemyLostEvent : UnityEvent { }
        // public readonly EnemyLostEvent enemyLost = new EnemyLostEvent();

        // public void Setup(GameplayManager gameplayManager, AgentsManager.Party party)
        // {
        //     this.gameplayManager = gameplayManager;
        //     this.party = party;

        //     agentMovement = GetComponent<AgentMovement>();
        //     agentMovement.Setup(this);

        //     agentHealth = GetComponent<AgentHealth>();
        //     agentHealth.Setup(this);

        //     combat = GetComponent<Combat>();
        //     combat.Setup(this);

        //     agentsDetection = GetComponentInChildren<AgentsDetection>();
        //     agentsDetection.Setup(this);

        //     var agentAI = GetComponent<AgentAI>();
        //     if (agentAI) agentAI.Setup(this);
        // }

        public void Setup(
            GameplayManager gameplayManager,
            GameCore.Agent agent
        )
        {
            this.gameplayManager = gameplayManager;
            this.agent = agent;

            this.agentAnimation = new AgentAnimation(
                agentController: this
            );

            var navMeshAgent = GetComponent<NavMeshAgent>();
            var agentMovement = new AgentMovement(
                agentController: this,
                navMeshAgent: navMeshAgent
            );

            var agentDetection = GetComponentInChildren<AgentsDetection>();
            agentDetection.Setup(
                gameplayManager: gameplayManager,
                agent: agent
            );
        }

        public void OnSpawned(SpriteModel spriteModel)
        {
            if (spriteModel.Animator != null)
            {
                agentAnimation.OnSpawned(spriteModel);
            }
        }

        public void SetDestination(Vector2 pos) { agent.movement.SetDestination(pos); }
        public void CancelMovement() {}
        public void MarkArrived() { agent.movement.MarkArrived(); }

        public IReadOnlyList<GameCore.Skill> skills => agent.combat.Skills;
        public void SetActiveSkill(GameCore.Skill skill) { agent.combat.SetActiveSkill(skill); }
        public void Attack(Vector3 pos) { agent.combat.Attack(pos); }

        // public void AddVisibleEnemy(AgentController agent)
        // {
        //     visibleEnemies.Add(agent);
        //     agent.Health.died.AddListener(OnEnemyDied);

        //     enemyDetected?.Invoke();
        // }

        // public void RemoveVisibleEnemy(AgentController agent)
        // {
        //     visibleEnemies.Remove(agent);
        //     agent.Health.died.RemoveListener(OnEnemyDied);

        //     enemyLost?.Invoke();
        // }

        // void OnEnemyDied(AgentController agent)
        // {
        //     RemoveVisibleEnemy(agent);
        // }
    }
}
