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
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, fieldSize);
        }
    }
}
