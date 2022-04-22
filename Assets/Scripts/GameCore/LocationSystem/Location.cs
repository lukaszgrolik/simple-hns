using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class Location
    {
        public readonly LocationSystem locationSystem;
        public readonly DataDefinition.Location data;

        public List<Agent> Agents => locationSystem.agentStore.GetAgentsInLocation(this);

        public event System.Action<Location, Agent> entered;

        // public readonly entrances

        public Location(
            LocationSystem locationSystem,
            DataDefinition.Location data
        )
        {
            this.locationSystem = locationSystem;
            this.data = data;
        }

        public void OnAgentEntered(Agent agent)
        {
            entered?.Invoke(this, agent);
        }
    }
}