using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonoBehaviors
{
    public class SpriteModel : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        public Animator Animator => animator;

        public void OnSpawned()
        {

        }
    }
}
