using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonoBehaviors
{
    public class Combat : MonoBehaviour
    {
        AgentController agentController;
        public AgentController AgentController => agentController;

        [SerializeField] Transform projectileSpawnPoint;
        public Transform ProjectileSpawnPoint => projectileSpawnPoint;

        public void Setup(AgentController agentController)
        {
            this.agentController = agentController;

        }
    }
}
