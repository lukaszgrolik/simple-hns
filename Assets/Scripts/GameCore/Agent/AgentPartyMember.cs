using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameCore
{
    public class AgentPartyMember
    {
        public readonly Game game;
        private DataDefinition.AgentParty agentPartyData; public DataDefinition.AgentParty AgentPartyData => agentPartyData;

        public class ChangedPartyEvent : UnityEvent<Agent> { }
        public readonly ChangedPartyEvent changedParty = new ChangedPartyEvent();

        public AgentPartyMember(
            Game game,
            DataDefinition.AgentParty agentPartyData
        )
        {
            this.game = game;
            this.agentPartyData = agentPartyData;
        }

        void SetParty(DataDefinition.AgentParty agentPartyData)
        {
            this.agentPartyData = agentPartyData;

            changedParty.Invoke(agent);
        }
    }
}