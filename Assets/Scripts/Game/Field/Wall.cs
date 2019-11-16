using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class Wall : MonoBehaviour
    {
        [SerializeField] private Vector2Int m_size = Vector2Int.one;
        [SerializeField] private Vector2Int m_location = Vector2Int.zero;
        [SerializeField] private bool m_useCurrentPosition = true;

        private FieldTransform m_fieldTransform => this.GetCachedComponent<FieldTransform>();
        private Field m_field => this.GetCachedComponentInParent<Field>();

        public Vector2Int gridSize => m_size;
        public Vector2 centerLocation => GetLocation() + 0.5f * (Vector2)m_size - Vector2.one * 0.5f;

        private void OnEnable()
        {
            var location = GetLocation();
            m_fieldTransform.location = location;

            for (var c = 0; c < m_size.x; c++)
                for (var r = 0; r < m_size.y; r++)
                    m_field.SetTileType(location + new Vector2Int(c, r), Field.TileType.Wall);
        }

        private Vector2Int GetLocation()
        {
            if (m_useCurrentPosition)
                return m_fieldTransform.GetSnappedLocation();
            return m_location;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            m_field.DrawGridGizmos(centerLocation, m_size);
        }
    }
}
