using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AgentPartyMember : AgentComponent
    {
        public readonly Game game;

        private AgentsParty agentsParty;
        public AgentsParty AgentsParty => agentsParty;

        public event System.Action<Agent> changedParty;

        public AgentPartyMember(
            Game game,
            AgentsParty agentsParty
        )
        {
            this.game = game;
            this.agentsParty = agentsParty;
        }

        void SetParty(AgentsParty agentsParty)
        {
            this.agentsParty = agentsParty;

            changedParty?.Invoke(agent);
        }
    }
}