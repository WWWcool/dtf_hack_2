using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class InitialVelocity : MonoBehaviour
    {
        [SerializeField] private Vector2 m_velocity;

        private void Start()
        {
            this.GetCachedComponent<Rigidbody2D>().velocity = m_velocity;
        }
    }
}
