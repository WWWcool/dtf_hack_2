using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class Field : MonoBehaviour
    {
        [SerializeField] private Vector2Int m_resolution;
        [SerializeField] private Vector2 m_tileSize;

        public Vector2Int resolution => m_resolution;
        public Vector2 tileSize => m_tileSize;
        public Vector2 fieldSize => m_resolution * m_tileSize;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
            for (var c = 0; c < m_resolution.x + 1; c++)
            {
                var from = GetPositionAtLocation(new Vector2(c - 0.5f, -0.5f));
                var to = GetPositionAtLocation(new Vector2(c - 0.5f, m_resolution.y - 0.5f));
                Gizmos.DrawLine(from, to);
            }

            for (var r = 0; r < m_resolution.y + 1; r++)
            {
                var from = GetPositionAtLocation(new Vector2(-0.5f, r - 0.5f));
                var to = GetPositionAtLocation(new Vector2(m_resolution.x - 0.5f, r - 0.5f));
                Gizmos.DrawLine(from, to);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, fieldSize);
        }

        public Vector2 GetLocationAtPosition(Vector2 position)
        {
            position -= 0.5f * m_tileSize;
            position += 0.5f * fieldSize;
            return position / m_tileSize;
        }

        public Vector2 GetPositionAtLocation(Vector2 location)
        {
            var position = location * m_tileSize;
            position -= 0.5f * fieldSize;
            position += 0.5f * m_tileSize;
            return position;
        }
    }
}
