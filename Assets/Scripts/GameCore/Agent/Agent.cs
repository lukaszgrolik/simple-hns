using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class AgentComponent
    {
        protected Agent agent;
        public Agent Agent => agent;

        public void SetAgent(Agent agent)
        {
            if (this.agent != null) throw new System.Exception("agent already set");
            this.agent = agent;
        }
    }

    // @todo join two groups if they have small amount of members?
    public class AgentsGroup
    {
        private Game game;

        public readonly List<Agent> members = new List<Agent>();

        public AgentsGroup(Game game)
        {
            this.game = game;
        }

        public void AddMember(Agent agent)
        {
            members.Add(agent);
        }

        public Vector3 GetCenter()
        {
            var x = 0f;
            var z = 0f;

            for (int i = 0; i < members.Count; i++)
            {
                var pos = game.GetPosition(members[i]);
                x += pos.x / members.Count;
                z += pos.z / members.Count;
            }

            return new Vector3(x, 0, z);
        }
    }

    public class AgentGroupMember
    {
        private AgentsGroup agentsGroup;
        public AgentsGroup AgentsGroup => agentsGroup;

        public AgentGroupMember(AgentsGroup agentsGroup)
        {
            this.agentsGroup = agentsGroup;
        }

        // public void SetAgentsGroup(AgentsGroup agentsGroup)
        // {
        //     this.agentsGroup = agentsGroup;
        // }
    }

    public class Agent : ITransformScript
    {
        public readonly Game game;

        public readonly AgentGroupMember groupMember;
        public readonly AgentEquipment equipment;
        public readonly AgentHealth health;
        public readonly AgentDrop drop;
        public readonly AgentMovement movement;
        public readonly AgentPartyMember partyMember;
        public readonly AgentDetection agentDetection;
        public readonly AgentCombat combat;
        public readonly AgentControl control;

        public readonly DataDefinition.Agent agentData;

        public Agent(
            Game game,
            AgentGroupMember groupMember,
            AgentEquipment equipment,
            AgentHealth health,
            AgentDrop drop,
            AgentMovement movement,
            AgentPartyMember partyMember,
            AgentDetection agentDetection,
            AgentCombat combat,
            AgentControl control,
            DataDefinition.Agent agentData
        )
        {
            this.game = game;

            this.groupMember = groupMember;

            this.equipment = equipment;
            equipment.SetAgent(this);

            this.health = health;
            health.SetAgent(this);

            this.drop = drop;

            this.movement = movement;

            this.partyMember = partyMember;
            partyMember.SetAgent(this);

            this.agentDetection = agentDetection;
            agentDetection.SetAgent(this);

            this.combat = combat;
            combat.SetAgent(this);

            this.control = control;
            control.SetAgent(this);

            this.agentData = agentData;
        }

        public void Setup()
        {
            control.Setup();
        }

        public void OnUpdate(float deltaTime)
        {
            combat.OnUpdate(deltaTime);

            if (control is IAgentControlTickable agentAIControl)
            {
                agentAIControl.OnUpdate();
            }
        }
    }
}