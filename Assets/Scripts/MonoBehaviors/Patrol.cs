using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace MonoBehaviors
{
    // public class Patrol : MonoBehaviour
    // {
    //     private float radius = 5f;
    //     // private float speed = 5f;
    //     // private float waitTime = 1f;

    //     // private Vector3 lastPos;
    //     private Vector3 nextDestination;
    //     // private float distance;
    //     private float remDist;
    //     private float lastRemDist;
    //     private float sameRemDistTime = 0;

    //     private AgentMovement agentMovement;

    //     public void Setup(AgentController agentController)
    //     {
    //         agentMovement = agentController.Movement;

    //         GoToRandomDestination();
    //     }

    //     void Update()
    //     {
    //         // travelProgress += navMeshAgent.speed * Time.deltaTime / distance;
    //         remDist = Vector3.Distance(transform.position, nextDestination);

    //         if (remDist == lastRemDist)
    //         {
    //             sameRemDistTime += Time.deltaTime;
    //         }
    //         else
    //         {
    //             lastRemDist = remDist;
    //             sameRemDistTime = 0;
    //         }

    //         // transform.position = Vector3.Lerp(lastPos, nextDestination, travelProgress);

    //         if (remDist < .5f || sameRemDistTime > 1f)
    //         {
    //             GoToRandomDestination();
    //         }
    //     }

    //     void GoToRandomDestination()
    //     {
    //         // lastPos = transform.position;
    //         nextDestination = GetRandomPos();
    //         // distance = Vector3.Distance(lastPos, nextDestination);

    //         agentMovement.SetDestination(nextDestination);
    //     }

    //     Vector3 GetRandomPos()
    //     {
    //         var randomPos = Random.insideUnitCircle * radius;

    //         return transform.position + new Vector3(randomPos.x, 0, randomPos.y);
    //     }

    //     void OnDrawGizmosSelected()
    //     {
    //         // Gizmos.DrawWireSphere(lastPos, radius);
    //         // Gizmos.DrawLine(lastPos, nextDestination);
    //     }
    // }
}