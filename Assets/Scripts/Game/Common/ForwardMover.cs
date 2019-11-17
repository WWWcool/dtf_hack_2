using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class ForwardMover : MonoBehaviour
    {
        [SerializeField] private float m_speed = 10.0f;

        private void FixedUpdate()
        {
            transform.position += transform.right * m_speed * Time.fixedDeltaTime;
        }
    }
}
