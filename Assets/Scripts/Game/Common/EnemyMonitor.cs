using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class EnemyMonitor : MonoBehaviour
    {
        [SerializeField] private GameObjectCollection m_enemies;

        private void OnEnable()
        {
            m_enemies.OnCollectionBecameEmpty += OnEnemiesDefeated;
        }

        private void OnDisable()
        {
            m_enemies.OnCollectionBecameEmpty -= OnEnemiesDefeated;
        }

        private void OnEnemiesDefeated()
        {
            EventBus.Instance.Raise(new GameEvents.OnGameOver { won = true });
        }
    }
}
