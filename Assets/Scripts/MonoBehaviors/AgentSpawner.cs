using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace MonoBehaviors
{
    public class AgentSpawner : MonoBehaviour
    {
        private GameCore.Game game;
        private GameplayManagerAgentEntities agentEntities;

        [SerializeField] private AgentType agentType;
        [SerializeField] private int min = 5;
        [SerializeField] private int max = 5;
        [SerializeField] private float radius = 5;

        public void SpawnAgents(
            GameCore.Game game,
            GameplayManagerAgentEntities agentEntities
        )
        {
            this.game = game;
            this.agentEntities = agentEntities;

            var agentsGroup = game.CreateAgentsGroup();

            var amount = Random.Range(min, max + 1);

            for (int i = 0; i < amount; i++)
            {
                SpawnAgent(agentsGroup);
            }
        }

        void SpawnAgent(GameCore.AgentsGroup agentsGroup)
        {
            var circlePos = Random.insideUnitCircle.normalized;
            var pos = transform.position + new Vector3(circlePos.x, 0, circlePos.y) * radius;
            var agent = agentEntities.CreateAgent(
                game: game,
                agentType: agentType,
                agentsGroup: agentsGroup
            );

            agentsGroup.AddMember(agent);

            game.SpawnAgent(agent, pos, Quaternion.identity);
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;

            var gameplayManager = FindObjectOfType<GameplayManager>();
            var agentModelPrefab = gameplayManager.FindAgentModelPrefab(agentType);

            System.Action InstantiateModel = () => {
                StartCoroutine(InstantiateCorout(agentModelPrefab, transform));
            };

            if (transform.childCount == 0)
            {
                InstantiateModel();
            }
            else
            {
                var obj = transform.GetChild(0);
                // var sceneAgent = obj.GetComponent<SceneAgent>();

                // if (sceneAgent.AgentType != agentType)
                // if (PrefabUtility.GetCorrespondingObjectFromSource(obj.gameObject) != agentModelPrefab)
                // {
                    StartCoroutine(DestroyImmediateCorout(obj.gameObject));
                    InstantiateModel();
                // }
            }
        }

        IEnumerator InstantiateCorout(GameObject go, Transform transform)
        {
            yield return new WaitForEndOfFrame();

            Instantiate(go, transform);
        }

        IEnumerator DestroyImmediateCorout(GameObject go)
        {
            yield return new WaitForEndOfFrame();

            DestroyImmediate(go);
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
            Gizmos.DrawSphere(transform.position, .15f);
            // Gizmos.DrawLine(transform.position, transform.position + Vector3.up * 5);
        }
#endif
    }
}
