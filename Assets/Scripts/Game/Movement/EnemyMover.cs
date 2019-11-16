using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class EnemyMover : MonoBehaviour
    {
        [System.Serializable]
        private struct MovementPattern
        {
            public FieldMover.Direction direction;
            public int length;
        }

        [SerializeField] private MovementPattern[] m_movementPatterns = new MovementPattern[0];

        private FieldMover m_mover => this.GetCachedComponent<FieldMover>();
        private MovementPattern m_currentPattern => m_movementPatterns[m_patternIndex];

        private int m_patternIndex = -1;
        private int m_stepsLeft = 0;

        private void OnEnable()
        {
            m_mover.onTileReached += OnTileReached;
        }

        private void OnDisable()
        {
            m_mover.onTileReached -= OnTileReached;
        }

        private void Start()
        {
            NextPattern(true);
        }

        public void OnTileReached()
        {
            m_stepsLeft--;
            if (m_stepsLeft <= 0)
                NextPattern(false);
        }

        public void NextPattern(bool force)
        {
            m_patternIndex = (m_patternIndex + 1) % m_movementPatterns.Length;
            m_stepsLeft = m_currentPattern.length;

            if (force)
                m_mover.currentDirection = m_currentPattern.direction;
            else
                m_mover.ScheduleTurn(m_currentPattern.direction);
        }
    }
}
