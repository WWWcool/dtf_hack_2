using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityPrototype
{
    public class CollisionDamageDealer : MonoBehaviour
    {
        [System.Serializable]
        private class DamageParameters
        {
            public float damage = 10.0f;
        }

        [SerializeField] private DamageParameters m_parameters;
        [SerializeField] private float m_collisionDelay = 0.1f;
        [SerializeField] private LayerMask m_targetLayers = new LayerMask { value = ~0 };
        [SerializeField] private UnityEventGameObject m_onCollision = default;
        [SerializeField] private UnityEventVector2 m_onCollisionPosition = default;
        [SerializeField] private UnityEventGameObjectVector2 m_onCollisionObjectPosition = default;

        public float damageCoefficient { get; set; } = 1.0f;

        private float m_cooldown = 0.0f;

        private void FixedUpdate()
        {
            var dt = Time.fixedDeltaTime;
            m_cooldown -= dt;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            ContactPoint2D invalidContact = new ContactPoint2D();
            ProcessCollision(other, invalidContact);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            ProcessCollision(other.collider, other.contacts[0]);
        }

        private void ProcessCollision(Collider2D otherCollider, ContactPoint2D contact)
        {
            if (m_cooldown > 0.0f)
                return;

            if (!enabled)
                return;

            var damageReceiver = otherCollider.gameObject.GetComponent<DamageReceiver>();

            var contactPoint = Vector2.zero;

            if (contact.collider != null)
            {
                contactPoint = contact.point;
                m_onCollisionPosition?.Invoke(contactPoint);
                m_onCollisionObjectPosition?.Invoke(otherCollider.gameObject, contactPoint);
            }

            if (damageReceiver)
                damageReceiver.ReceiveDamage(m_parameters.damage * damageCoefficient, contactPoint);

            m_onCollision?.Invoke(otherCollider.gameObject);
            m_cooldown = m_collisionDelay;
        }
    }
}
