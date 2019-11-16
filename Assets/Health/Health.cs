using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityPrototype
{
    public class Health : MonoBehaviour
    {
        [System.Serializable]
        private class HealthParameters
        {
            public float maxHealth = 100.0f;
            public bool isImmortal = false;
        }

        [SerializeField] private HealthParameters m_parameters;
        [SerializeField] private UnityEvent m_onHealthDepleted = default;
        [SerializeField] private UnityEventFloat m_onHealthChanged = default;

        private float m_health;
        private bool m_dead = false;

        public float maxHealth { get { return m_parameters.maxHealth; } }
        public float health { get { return m_health; } }
        public bool isAlive { get { return !m_dead; } }

        private void Awake()
        {
            m_health = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (m_parameters.isImmortal)
                return;

            UpdateHealth(m_health - damage);
        }

        public void RegenerateHealth(float hp, bool notifyHealthChanged = true)
        {
            if (m_parameters.isImmortal)
                return;

            UpdateHealth(m_health + hp, notifyHealthChanged);
        }

        private void UpdateHealth(float targetHealth, bool notifyHealthChanged = true)
        {
            if (m_dead)
                return;

            var newHealth = Mathf.Clamp(targetHealth, 0.0f, maxHealth);
            var delta = newHealth - m_health;
            m_health = newHealth;

            if (notifyHealthChanged)
                m_onHealthChanged?.Invoke(delta);

            if (Mathf.Abs(m_health) < Mathf.Epsilon)
            {
                m_onHealthDepleted?.Invoke();
                m_dead = true;
            }
        }

        public void InstaKill()
        {
            UpdateHealth(0.0f);
        }

        public void Leave1HP()
        {
            UpdateHealth(0.00001f * maxHealth, false);
        }
    }
}
