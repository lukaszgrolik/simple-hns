using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndexedList<TKey, TVal> {
    private List<TVal> list = new List<TVal>();
    private Dictionary<TKey, TVal> dict = new Dictionary<TKey, TVal>();

    public void Add(TKey key, TVal val) {
        list.Add(val);
        dict.Add(key, val);
    }

    public void Remove(TKey key) {
        var val = dict[key];

        list.Remove(val);
        dict.Remove(key);
    }

    public bool TryGetValue(TKey key, out TVal val) {
        return dict.TryGetValue(key, out val);
    }
}

public class AgentsManager : MonoBehaviour {
    public class Party {

    }

    private List<Party> parties = new List<Party>(){
        new Party(),
        new Party(),
    };
    public List<Party> Parties => parties;

    private IndexedList<GameObject, AgentController> agents = new IndexedList<GameObject, AgentController>();
    public IndexedList<GameObject, AgentController> Agents => agents;

    private IndexedList<GameObject, AgentController> aliveAgents = new IndexedList<GameObject, AgentController>();
    public IndexedList<GameObject, AgentController> AliveAgents => aliveAgents;

    private Dictionary<Party, IndexedList<GameObject, AgentController>> aliveEnemies = new Dictionary<Party, IndexedList<GameObject, AgentController>>();
    public Dictionary<Party, IndexedList<GameObject, AgentController>> AliveEnemies => aliveEnemies;

    public void Setup() {
        foreach (var party in parties) {
            aliveEnemies.Add(party, new IndexedList<GameObject, AgentController>());
        }
    }

    public void RegisterAgent(AgentController agent) {
        agents.Add(agent.gameObject, agent);
        aliveAgents.Add(agent.gameObject, agent);

        foreach (var party in aliveEnemies) {
            if (party.Key != agent.Party) {
                party.Value.Add(agent.gameObject, agent);
            }
        }

        agent.Health.Died.AddListener(OnAgentDied);
    }

    void OnAgentDied(AgentController agent) {
        agents.Remove(agent.gameObject);
        aliveAgents.Remove(agent.gameObject);

        foreach (var party in aliveEnemies) {
            if (party.Key != agent.Party) {
                party.Value.Remove(agent.gameObject);
            }
        }
    }

    void OnAgentChangedParty() {
        throw new System.NotImplementedException();
    }

    void HandlePartyAdded() {
        throw new System.NotImplementedException();
    }

    void HandlePartyRemoved() {
        throw new System.NotImplementedException();
    }
}
