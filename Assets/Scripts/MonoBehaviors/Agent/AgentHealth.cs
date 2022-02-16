using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

// namespace MonoBehaviors
// {
//     public class AgentHealth : MonoBehaviour
//     {
//         private AgentController agentController;

//         int maxPoints = 100;
//         public int MaxPoints => maxPoints;

//         int currentPoints = 100;
//         public int CurrentPoints => currentPoints;

//         public class HealthChangedEvent : UnityEvent<int, int> { }
//         public readonly HealthChangedEvent healthChanged = new HealthChangedEvent();

//         public class DiedEvent : UnityEvent<AgentController> { }
//         public readonly DiedEvent died = new DiedEvent();

//         public void Setup(AgentController agentController)
//         {
//             this.agentController = agentController;
//         }

//         public void TakeDamage(int damagePoints)
//         {
//             currentPoints -= damagePoints;
//             if (currentPoints < 0) currentPoints = 0;

//             healthChanged.Invoke(currentPoints, maxPoints);

//             if (currentPoints == 0)
//             {
//                 Die();
//             }
//         }

//         void Die()
//         {
//             died.Invoke(agentController);
//         }

// #if UNITY_EDITOR
//         void OnDrawGizmos()
//         {
//             Handles.Label(transform.position, currentPoints.ToString());
//         }
// #endif
//     }

// }
