using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class ObjectDestroyer : MonoBehaviour
    {
        [SerializeField] private float m_delay = 0.0f;
        [SerializeField] private bool m_alignExplosion = false;
        [SerializeField] private GameObject m_explosionPrefab = null;
        [SerializeField] private bool m_scheduleSpawnOnStart = false;

        private bool m_destructionScheduled = false;

        private void Start()
        {
            if (m_scheduleSpawnOnStart)
                Destroy();
        }

        public void Destroy()
        {
            if (m_destructionScheduled)
            {
                Debug.LogWarning("Trying to destroy object for the second time");
                return;
            }

            m_destructionScheduled = true;
            StartCoroutine(DestroyDelayed());
        }

        private IEnumerator DestroyDelayed()
        {
            var delay = m_delay;

            while (delay > 0.0f)
            {
                yield return new WaitForFixedUpdate();
                delay -= Time.fixedDeltaTime;
            }

            if (m_explosionPrefab != null)
                Instantiate(m_explosionPrefab, transform.position, m_alignExplosion ? transform.rotation : Quaternion.identity);

            Destroy(gameObject);
        }
    }
}
