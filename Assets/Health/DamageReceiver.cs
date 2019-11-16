using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class DamageReceiver : MonoBehaviour
    {
        [System.Serializable]
        private class DamageReceiverParameters
        {
            public float damageMultiplier = 1.0f;
        }

        [SerializeField] private DamageReceiverParameters m_parameters;
        [SerializeField] private UnityEventVector2 m_onDamagePositionReceived = default;
        [SerializeField] private bool m_debugReceive = false;

        private Health m_health;

        private float m_debugDamageTimer = 0;

        private void OnEnable()
        {
            m_health = GetComponentInParent<Health>();
            Debug.Assert(m_health);
        }

        public void ReceiveDamage(float damage, Vector2 position)
        {
            m_health.TakeDamage(damage * m_parameters.damageMultiplier);
            m_onDamagePositionReceived?.Invoke(position);
        }

        public void InstaKill()
        {
            m_health.InstaKill();
        }

        private void Update()
        {
            if (m_debugReceive)
            {
                m_debugDamageTimer += Time.deltaTime;
                if (m_debugDamageTimer >= 1)
                {
                    m_debugDamageTimer = 0;
                    m_health.TakeDamage(10);
                }
            }
        }
    }
}
