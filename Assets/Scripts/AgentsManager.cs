using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class CachedList<TKey, TVal> {
    private List<TVal> list = new List<TVal>();
    private Dictionary<TKey, TVal> dict = new Dictionary<TKey, TVal>();
}

public class AgentsManager : MonoBehaviour {
    public class Party {

    }

    private List<Party> parties = new List<Party>(){
        new Party(),
        new Party(),
    };
    public List<Party> Parties => parties;

    private CachedList<GameObject, AgentController> agents = new CachedList<GameObject, AgentController>();
    private CachedList<GameObject, AgentController> aliveAgents = new CachedList<GameObject, AgentController>();
    private Dictionary<Party, CachedList<GameObject, AgentController>> aliveEnemies = new Dictionary<Party, CachedList<GameObject, AgentController>>();

    public void Setup() {

    }
}
