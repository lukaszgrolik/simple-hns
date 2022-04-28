using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonoBehaviors
{
    public class Helper_HorizontalLine : MonoBehaviour
    {
        [SerializeField] private List<float> heights = new List<float>();
        [SerializeField] private float radius = 2f;

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            for (int i = 0; i < heights.Count; i++)
            {
                var y = heights[i];
                var pos = Vector3.zero.With(y: y);
                Gizmos.DrawLine(pos.With(x: pos.x - radius), pos.With(x: pos.x + radius));
            }
        }
#endif
    }
}