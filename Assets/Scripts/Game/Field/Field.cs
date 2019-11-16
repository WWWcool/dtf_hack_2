using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class Field : MonoBehaviour
    {
        public enum TileType
        {
            Empty,
            Wall,
        }

        [System.Serializable]
        public struct Tile
        {
            public TileType type;
        }

        [SerializeField] private Vector2Int m_resolution;
        [SerializeField] private Vector2 m_tileSize;

        public Vector2Int resolution => m_resolution;
        public Vector2 tileSize => m_tileSize;
        public Vector2 fieldSize => m_resolution * m_tileSize;

        private Tile[,] m_cachedGrid = null;
        private Tile[,] m_grid
        {
            get
            {
                if (m_cachedGrid == null)
                    InitGrid();

                return m_cachedGrid;
            }
        }

        private void InitGrid()
        {
            m_cachedGrid = new Tile[m_resolution.x, m_resolution.y];

            for (var c = 0; c < m_resolution.x; c++)
                for (var r = 0; r < m_resolution.y; r++)
                    m_cachedGrid[r, c] = new Tile { type = TileType.Empty };
        }

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

        public void DrawGridGizmos(Vector2 location, Vector2 gridSize)
        {
            var pos = GetPositionAtLocation(location);
            Gizmos.DrawWireCube(pos, GetSize(gridSize));
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

        public Vector2 GetSize(Vector2 gridSize)
        {
            return gridSize * m_tileSize;
        }

        public void SetTileType(Vector2Int location, TileType type)
        {
            m_grid[location.x, location.y].type = type;
        }

        public bool IsPassable(Vector2Int location)
        {
            return m_grid[location.x, location.y].type == TileType.Empty;
        }
    }
}
