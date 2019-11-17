using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataStructures.Graphs;
using Algorithms.Graphs;
using System;

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

        private class TileGraphNode : IComparable<TileGraphNode>
        {
            public TileGraphNode(Vector2Int location)
            {
                this.location = location;
            }

            public Vector2Int location;

            public int CompareTo(TileGraphNode other)
            {
                return Compare(location, other.location);
            }

            private static int Compare(Vector2Int lhs, Vector2Int rhs)
            {
                if (lhs.x == rhs.x)
                    return lhs.y.CompareTo(rhs.y);

                return lhs.x.CompareTo(rhs.x);
            }
        }

        public class PathData
        {
            public List<Vector2Int> waypoints = new List<Vector2Int>();
        }

        [SerializeField] private Vector2Int m_resolution;
        [SerializeField] private Vector2 m_tileSize;
        [SerializeField] private GameObjectCollection m_players;

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

        private UndirectedSparseGraph<TileGraphNode> m_graph = null;
        private Dictionary<Vector2Int, TileGraphNode> m_nodeMap = new Dictionary<Vector2Int, TileGraphNode>();

        private void InitGrid()
        {
            m_graph = new UndirectedSparseGraph<TileGraphNode>((uint)(m_resolution.x * m_resolution.y));
            m_nodeMap.Clear();
            m_cachedGrid = new Tile[m_resolution.x, m_resolution.y];

            for (var c = 0; c < m_resolution.x; c++)
            {
                for (var r = 0; r < m_resolution.y; r++)
                {
                    var location = new Vector2Int(c, r);
                    var node = new TileGraphNode(location);
                    m_graph.AddVertex(node);
                    m_nodeMap[location] = node;

                    m_cachedGrid[c, r] = new Tile { type = TileType.Empty };
                }
            }

            for (var c = 0; c < m_resolution.x; c++)
                for (var r = 0; r < m_resolution.y; r++)
                    AddEdgesForLocation(new Vector2Int(c, r));
        }

        private void AddEdgesForLocation(Vector2Int location)
        {
            var node = GetNode(location);
            var neighbors = GetNeighbors(location);

            foreach (var neighbor in neighbors)
                m_graph.AddEdge(node, neighbor);
        }

        private void RemoveEdgesForLocation(Vector2Int location)
        {
            var node = GetNode(location);
            var neighbors = GetNeighbors(location);

            foreach (var neighbor in neighbors)
                m_graph.RemoveEdge(node, neighbor);
        }

        private IEnumerable<TileGraphNode> GetNeighbors(Vector2Int location)
        {
            var offsets = new List<Vector2Int>
            {
                new Vector2Int(-1, 0),
                new Vector2Int(1, 0),
                new Vector2Int(0, -1),
                new Vector2Int(0, 1),
            };

            var neighbors = new List<TileGraphNode>();

            foreach (var offset in offsets)
            {
                var node = GetNode(location + offset);

                if (node == null)
                    continue;

                // var tile = node.slot.childTile;

                if (!IsPassable(node.location))
                    continue;

                // if (tile == null)
                //     continue;

                neighbors.Add(node);
            }

            return neighbors;
        }

        private TileGraphNode GetNode(Vector2Int location)
        {
            var expectedSize = m_resolution.x * m_resolution.y;
            Debug.Assert(m_nodeMap.Count == expectedSize, $"Node map doesn't contain needed all the tiles. Size is {m_nodeMap.Count}, expected {expectedSize}");

            if (m_nodeMap.TryGetValue(location, out var node))
                return node;

            return null;
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

            DragGraphGizmos();
        }

        private void DragGraphGizmos()
        {
            if (m_graph == null)
                return;

            Gizmos.color = Color.black;
            foreach (var v in m_graph.Vertices)
            {
                Gizmos.DrawSphere(GetPositionAtLocation(v.location), 0.1f);
            }

            foreach (var e in m_graph.Edges)
            {
                var from = GetPositionAtLocation(e.Source.location);
                var to = GetPositionAtLocation(e.Destination.location);

                Gizmos.DrawLine(from, to);
            }
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

            if (type == TileType.Wall)
                RemoveEdgesForLocation(location);
        }

        public bool IsPassable(Vector2Int location)
        {
            return m_grid[location.x, location.y].type == TileType.Empty;
        }

        private int ManhattanDistance(Vector2Int fromLocation, Vector2Int toLocation)
        {
            var diff = toLocation - fromLocation;
            return Mathf.Abs(diff.x) + Mathf.Abs(diff.y);
        }

        public PathData FindPathToPlayer(FieldTransform fieldTransform)
        {
            var fromLocation = fieldTransform.snappedLocation;
            var toLocation = m_players.GetAnyAlive().GetCachedComponent<FieldTransform>().snappedLocation;
            return FindPath(fromLocation, toLocation);
        }

        public PathData FindPath(Vector2Int fromLocation, Vector2Int toLocation)
        {
            var from = GetNode(fromLocation);
            var to = GetNode(toLocation);

            if (from == null || to == null)
                return CreatePathData(null);

            bool hasSourceNode = m_graph.HasVertex(from);
            bool hasDestinationNode = m_graph.HasVertex(to);

            Debug.Assert(hasSourceNode, $"Field doesn't contain a tile at '{fromLocation}' (source)");
            Debug.Assert(hasDestinationNode, $"Field doesn't contain a tile at '{toLocation}' (destination)");

            if (!hasSourceNode || !hasDestinationNode)
                return CreatePathData(null);

            var pathSearcher = new BreadthFirstShortestPaths<TileGraphNode>(m_graph, from);
            // var pathSearcher = new BellmanFordShortestPaths<UndirectedWeightedSparseGraph<TileGraphNode>, TileGraphNode>(m_graph, from);

            if (pathSearcher.HasPathTo(to))
                return CreatePathData(pathSearcher.ShortestPathTo(to));

            TileGraphNode closestNode = null;
            int closestDistance = int.MaxValue;

            foreach (var nodePair in m_nodeMap)
            {
                var location = nodePair.Key;
                var node = nodePair.Value;

                if (!pathSearcher.HasPathTo(node))
                    continue;

                var distance = ManhattanDistance(location, toLocation);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestNode = node;
                }
            }

            Debug.Assert(pathSearcher.HasPathTo(closestNode), "Path should exist");
            return CreatePathData(pathSearcher.ShortestPathTo(closestNode));
        }

        private PathData CreatePathData(IEnumerable<TileGraphNode> graphPath)
        {
            return CreatePathData(new List<TileGraphNode>(graphPath));
        }

        private PathData CreatePathData(List<TileGraphNode> graphPath)
        {
            if (graphPath == null)
                return null;

            var pathData = new PathData();

            bool shouldSkip = graphPath.Count > 1;

            foreach (var node in graphPath)
            {
                if (shouldSkip)
                {
                    shouldSkip = false;
                    continue;
                }

                pathData.waypoints.Add(node.location);
            }

            Debug.Assert(pathData.waypoints.Count > 0);

            return pathData;
        }
    }
}
