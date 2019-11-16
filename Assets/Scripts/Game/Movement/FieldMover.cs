using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class FieldMover : MonoBehaviour
    {
        [SerializeField] private float m_speed = 1.0f;

        private static readonly Dictionary<FieldMover.Direction, Vector2Int> m_directionVectors = new Dictionary<FieldMover.Direction, Vector2Int> {
            {FieldMover.Direction.Left, Vector2Int.left},
            {FieldMover.Direction.Right, Vector2Int.right},
            {FieldMover.Direction.Up, Vector2Int.up},
            {FieldMover.Direction.Down, Vector2Int.down},
        };

        private Direction m_currentDirection = Direction.Right;
        private Direction? m_scheduledDirection = null;

        private FieldTransform m_fieldTransform => this.GetCachedComponent<FieldTransform>();

        public enum Direction
        {
            Left,
            Right,
            Up,
            Down,
        }

        private void Start()
        {
            Snap();
        }

        private void Snap()
        {
            m_fieldTransform.Snap();
        }

        private static bool IsDirectionVertical(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                case Direction.Down:
                    return true;
            }

            return false;
        }

        private static bool IsDirectionFlipped(Direction dir)
        {
            switch (dir)
            {
                case Direction.Left:
                case Direction.Down:
                    return true;
            }

            return false;
        }

        private static Direction ReverseDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.Left:
                    return Direction.Right;
                case Direction.Right:
                    return Direction.Left;
                case Direction.Up:
                    return Direction.Down;
                case Direction.Down:
                    return Direction.Up;
            }

            Debug.Assert(false);
            return Direction.Right;
        }

        public void ScheduleTurn(Direction direction)
        {
            if (m_scheduledDirection.HasValue)
                Debug.LogWarning("Direction has been already scheduled");

            m_scheduledDirection = direction;
        }

        private void FixedUpdate()
        {
            var dt = Time.fixedDeltaTime;
            var timeUtilNextTile = DistanceUntilNextTile() / m_speed;

            var reachedNewTile = timeUtilNextTile <= dt;

            var dt1 = Mathf.Min(timeUtilNextTile, dt);
            var dt2 = dt - dt1;
            MoveInCurrentDirection(dt1);
            if (reachedNewTile)
                OnTileReached();
            MoveInCurrentDirection(dt2);
        }

        private void MoveInCurrentDirection(float dt)
        {
            var delta = (Vector2)GetDirectionVector(m_currentDirection) * m_speed * dt;
            m_fieldTransform.location += delta;
        }

        private void OnTileReached()
        {
            Snap();
            TrySwitchScheduledDirection();
            TryReflectOffWalls();
            EventBus.Instance.Raise(new GameEvents.OnTileReached() { currPosition = transform.position });
        }

        private void TrySwitchScheduledDirection()
        {
            if (m_scheduledDirection.HasValue)
                m_currentDirection = m_scheduledDirection.Value;
            m_scheduledDirection = null;
        }

        private void TryReflectOffWalls()
        {
            var nextLocation = GetNextTileLocation();
            if (this.GetCachedComponentInParent<Field>().IsPassable(nextLocation))
                return;

            m_currentDirection = ReverseDirection(m_currentDirection);
        }

        private Vector2Int GetDirectionVector(Direction dir)
        {
            return m_directionVectors[dir];
        }

        private Vector2Int GetNextTileLocation()
        {
            return m_fieldTransform.snappedLocation + GetDirectionVector(m_currentDirection);
        }

        private float DistanceUntilNextTile()
        {
            var loc = m_fieldTransform.location;
            var value = IsDirectionVertical(m_currentDirection) ? loc.y : loc.x;

            var d = 1.0f - Mathf.Repeat(value, 1.0f);
            if (IsDirectionFlipped(m_currentDirection))
                d = 1.0f - d;

            return d;
        }
    }
}
