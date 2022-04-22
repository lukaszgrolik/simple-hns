using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class LocationAgentStore
    {
        private readonly Dictionary<Location, List<Agent>> dict_location_agents = new Dictionary<Location, List<Agent>>();
        private readonly Dictionary<Agent, Location> dict_agent_location = new Dictionary<Agent, Location>();

        public List<Agent> GetAgentsInLocation(Location loc)
        {
            return dict_location_agents[loc];
        }

        public Location GetAgentLocation(Agent agent)
        {
            return dict_agent_location[agent];
        }

        public void AssignAgentsToLocation(Location location, List<Agent> agents)
        {
            // first remove agents from their previous locations
            for (int i = 0; i < agents.Count; i++)
            {
                // do nothing if agent has no location yet
                if (dict_agent_location.TryGetValue(agents[i], out var loc))
                {
                    var prevLocAgents = dict_location_agents[loc];
                    prevLocAgents.Remove(agents[i]);
                }
            }

            if (dict_location_agents.TryGetValue(location, out var locAgents))
            {
                locAgents.AddRange(agents);
            }
            else
            {
                dict_location_agents.Add(location, agents);
            }

            for (int i = 0; i < agents.Count; i++)
            {
                if (dict_agent_location.ContainsKey(agents[i]))
                {

                    dict_agent_location[agents[i]] = location;
                }
                else
                {
                    dict_agent_location.Add(agents[i], location);
                }
            }
        }

        // @todo remove agent from location when deleted (not on death)
        void RemoveAgent()
        {

        }
    }

    public class LocationSystem
    {
        public readonly List<Location> locations = new List<Location>();
        public readonly LocationAgentStore agentStore = new LocationAgentStore();

        public event System.Action<Location, Agent> locationEnteredByAgent;

        public LocationSystem()
        {

        }

        public void SetLocations(List<Location> locations)
        {
            this.locations.AddRange(locations);

            foreach (var loc in locations)
            {
                loc.entered += OnLocationEntered;
            }
        }

        // location, entrance
        public void EnterLocation()
        {

        }

        void OnLocationEntered(Location location, Agent agent)
        {
            agentStore.AssignAgentsToLocation(location, new List<Agent>(){agent});

            locationEnteredByAgent?.Invoke(location, agent);
        }
    }
}