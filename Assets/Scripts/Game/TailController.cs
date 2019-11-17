using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class TailController : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;

        private void OnEnable()
        {
            m_animator.Play("Idle");
            Destroy(gameObject, TailSpawnManager.s_destroyTime);
        }
    }
}
