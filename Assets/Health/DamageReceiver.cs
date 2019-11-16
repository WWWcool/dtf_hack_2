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

        private Health m_health;

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
    }
}
