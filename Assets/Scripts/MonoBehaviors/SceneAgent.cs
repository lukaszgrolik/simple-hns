using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviors
{
    public class SceneAgent : MonoBehaviour
    {
        [SerializeField] private AgentType agentType;
        public AgentType AgentType => agentType;
    }
}