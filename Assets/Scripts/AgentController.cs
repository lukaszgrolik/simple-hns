using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AgentController : MonoBehaviour {
    private GameplayManager gameplayManager;
    public GameplayManager GameplayManager => gameplayManager;

    private AgentMovement agentMovement;
    public AgentMovement Movement => agentMovement;

    private AgentHealth agentHealth;
    public AgentHealth Health => agentHealth;

    private Combat combat;
    public Combat Combat => combat;

    private AgentsDetection agentsDetection;
    public AgentsDetection AgentsDetection => agentsDetection;

    private AgentsManager.Party party;
    public AgentsManager.Party Party => party;

    public IndexedList<GameObject, AgentController> AliveEnemies => gameplayManager.AgentsManager.AliveEnemies[party];

    private List<AgentController> visibleEnemies = new List<AgentController>();
    public List<AgentController> VisibleEnemies => visibleEnemies;

    public class EnemyDetectedEvent : UnityEvent {}
    private EnemyDetectedEvent enemyDetected = new EnemyDetectedEvent();
    public EnemyDetectedEvent EnemyDetected => enemyDetected;

    public class EnemyLostEvent : UnityEvent { }
    private EnemyLostEvent enemyLost = new EnemyLostEvent();
    public EnemyLostEvent EnemyLost => enemyLost;

    public void Setup(GameplayManager gameplayManager, AgentsManager.Party party) {
        this.gameplayManager = gameplayManager;
        this.party = party;

        agentMovement = GetComponent<AgentMovement>();
        agentMovement.Setup(this);

        agentHealth = GetComponent<AgentHealth>();
        agentHealth.Setup(this);

        combat = GetComponent<Combat>();
        combat.Setup(this);

        agentsDetection = GetComponentInChildren<AgentsDetection>();
        agentsDetection.Setup(this);

        var agentAI = GetComponent<AgentAI>();
        if (agentAI) agentAI.Setup(this);
    }

    public void AddVisibleEnemy(AgentController agent) {
        visibleEnemies.Add(agent);
        agent.Health.Died.AddListener(OnEnemyDied);

        enemyDetected.Invoke();
    }

    public void RemoveVisibleEnemy(AgentController agent) {
        visibleEnemies.Remove(agent);
        agent.Health.Died.RemoveListener(OnEnemyDied);

        enemyLost.Invoke();
    }

    void OnEnemyDied(AgentController agent) {
        RemoveVisibleEnemy(agent);
    }
}
