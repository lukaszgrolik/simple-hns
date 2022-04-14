using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MonoBehaviors
{
    public class SceneAgent : MonoBehaviour
    {
        [SerializeField] private AgentType agentType;
        public AgentType AgentType => agentType;

    #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, .5f);
        }
    #endif
    }
}