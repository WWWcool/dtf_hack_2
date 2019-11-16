using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityPrototype
{
    public class CollisionInstantKiller : MonoBehaviour
    {
        [SerializeField] private UnityEvent m_onCollided = default;

        private void OnTriggerEnter2D(Collider2D other)
        {
            ProcessCollision(other);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            ProcessCollision(other.collider);
        }

        private void ProcessCollision(Collider2D otherCollider)
        {
            if (!enabled)
                return;

            var damageReceiver = otherCollider.gameObject.GetComponent<DamageReceiver>();

            if (damageReceiver)
                damageReceiver.InstaKill();

            m_onCollided?.Invoke();
        }
    }
}
