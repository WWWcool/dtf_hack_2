using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class FieldMover : MonoBehaviour
    {
        [SerializeField] private float m_speed = 1.0f;

        private static readonly Dictionary<FieldMover.Direction, Vector2> m_directionVectors = new Dictionary<FieldMover.Direction, Vector2> {
            {FieldMover.Direction.Left, Vector2.left},
            {FieldMover.Direction.Right, Vector2.right},
            {FieldMover.Direction.Up, Vector2.up},
            {FieldMover.Direction.Down, Vector2.down},
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

        private bool IsVertical(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up:
                case Direction.Down:
                    return true;
            }

            return false;
        }

        private bool IsFlipped(Direction dir)
        {
            switch (dir)
            {
                case Direction.Left:
                case Direction.Down:
                    return true;
            }

            return false;
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
            var delta = GetDirectionVector(m_currentDirection) * m_speed * dt;
            m_fieldTransform.location += delta;
        }

        private void OnTileReached()
        {
            Snap();
            TrySwitchDirection();
        }

        private void TrySwitchDirection()
        {
            if (m_scheduledDirection.HasValue)
                m_currentDirection = m_scheduledDirection.Value;
            m_scheduledDirection = null;
        }

        private Vector2 GetDirectionVector(Direction dir)
        {
            return m_directionVectors[dir];
        }

        private float DistanceUntilNextTile()
        {
            var loc = m_fieldTransform.location;
            var value = IsVertical(m_currentDirection) ? loc.y : loc.x;

            var d = 1.0f - Mathf.Repeat(value, 1.0f);
            if (IsFlipped(m_currentDirection))
                d = 1.0f - d;

            return d;
        }
    }
}
