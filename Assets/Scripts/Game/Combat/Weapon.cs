using System.Collections;
using System.Collections.Generic;
using Gamelogic.Extensions;
using UnityEngine;

namespace UnityPrototype
{
    public class Weapon : MonoBehaviour
    {
        [System.Serializable]
        public class WeaponParameters
        {
            public float reloadTime = 1.0f;
            public float spreadConeAngle = 15.0f;
            public GameObject projectilePrefab = null;
        }

        [SerializeField] private GameObjectCollection m_players = null;
        [SerializeField] private GameObject m_spawnPoint = null;
        [SerializeField] private GameObject m_fxPrefab = null;
        [SerializeField] private WeaponParameters m_parameters = null;

        private float m_shootDelay = 0.0f;
        private GameObject m_lockedTarget = null;
        private int m_shotsLeft = 0;
        private System.Action m_onFinished = null;

        private GameObject FindTarget()
        {
            return m_players?.GetAnyAlive();
        }

        private void FixedUpdate()
        {
            if (m_shotsLeft <= 0)
                return;

            m_shootDelay -= Time.fixedDeltaTime;

            if (m_lockedTarget == null)
                m_lockedTarget = FindTarget();

            if (m_lockedTarget != null && m_shootDelay <= 0.0f)
                Shoot(m_lockedTarget);
        }

        private void Shoot(GameObject target)
        {
            Debug.Assert(m_onFinished != null);
            Debug.Assert(m_shotsLeft > 0);

            var targetPos = (Vector2)target.transform.position;
            var selfPos = (Vector2)transform.position;

            var direction = targetPos - selfPos;
            var spreadAngle = m_parameters.spreadConeAngle;
            var angleOffset = Random.Range(-spreadAngle, spreadAngle) * 0.5f;
            var originalRotation = Quaternion.FromToRotation(Vector2.right, direction);
            direction = direction.Rotate(angleOffset);

            var localSpawnOffset = m_spawnPoint.transform.localPosition;
            var spawnOffset = originalRotation * localSpawnOffset;

            var rotation = Quaternion.FromToRotation(Vector2.right, direction);
            var spawnPosition = selfPos + (Vector2)spawnOffset;
            var projectile = Instantiate(m_parameters.projectilePrefab, spawnPosition, rotation);

            if (m_fxPrefab != null)
                Instantiate(m_fxPrefab, spawnPosition, originalRotation);

            m_shootDelay = m_parameters.reloadTime;
            m_shotsLeft--;

            if (m_shotsLeft <= 0)
            {
                m_onFinished?.Invoke();
                m_onFinished = null;
                Debug.Log("Finish Shooting");
            }
        }

        public void StartShooting(AIActionAttackContext context)
        {
            Debug.Log("Start Shooting");
            m_shotsLeft = context.maxOrCount;
            m_onFinished = context.onFinished;
        }
    }
}
