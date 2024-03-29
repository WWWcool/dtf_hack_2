using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class FieldMover : MonoBehaviour
    {
        [SerializeField] private float m_speed = 1.0f;
        [SerializeField] private bool m_spawnTail = false;
        [SerializeField] private int m_dashLength = 3;
        [SerializeField] private int m_dashCharges = 0;
        [SerializeField] private float m_dashTimeRecharge = 0;

        private static readonly Dictionary<FieldMover.Direction, Vector2Int> m_directionVectors = new Dictionary<FieldMover.Direction, Vector2Int> {
            {FieldMover.Direction.Left, Vector2Int.left},
            {FieldMover.Direction.Right, Vector2Int.right},
            {FieldMover.Direction.Up, Vector2Int.up},
            {FieldMover.Direction.Down, Vector2Int.down},
        };

        public delegate void OnReverseDirection(Direction dir);
        public OnReverseDirection onReverseDirection = (dir) => { };
        public Direction currentDirection { get; set; } = Direction.Up;
        private Direction? m_scheduledDirection = null;

        private FieldTransform m_fieldTransform => this.GetCachedComponent<FieldTransform>();

        public event System.Action onTileReached;
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
            if (m_spawnTail)
            {
                EventBus.Instance.Raise(new GameEvents.OnDashRecharge() { newCount = m_dashCharges });
            }
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
            // if (m_scheduledDirection.HasValue)
            //     Debug.LogWarning("Direction has been already scheduled");

            m_scheduledDirection = direction;
        }

        public void TryDash()
        {
            if (m_dashCharges > 0)
            {
                var startPosition = m_fieldTransform.location;
                var newLocation = GetDirectionVector(currentDirection) * m_dashLength;
                var newTile = newLocation + m_fieldTransform.snappedLocation;
                var resolution = this.GetCachedComponentInParent<Field>().resolution;
                if (newTile.x >= resolution.x || newTile.y >= resolution.y ||
                    newTile.x < 0 || newTile.y < 0) return;
                if (this.GetCachedComponentInParent<Field>().IsPassable(newTile))
                {
                    Snap();
                    m_fieldTransform.location += newLocation - (Vector2)GetDirectionVector(currentDirection) * 0.3f;
                    m_dashCharges--;
                    EventBus.Instance.Raise(new GameEvents.OnDashRecharge() { newCount = m_dashCharges });
                    StartCoroutine(DashRechargeTimer());
                    EventBus.Instance.Raise(new GameEvents.OnDash(startPosition,
                        GetDirectionVector(currentDirection), m_fieldTransform.location));
                }
            }
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
            var delta = (Vector2)GetDirectionVector(currentDirection) * m_speed * dt;
            m_fieldTransform.location += delta;
        }

        private void OnTileReached()
        {

            Snap();
            onTileReached?.Invoke();
            TrySwitchScheduledDirection();
            TryReflectOffWalls();
            EventBus.Instance.Raise(new GameEvents.OnTileReached() { spawnTail = m_spawnTail, currPosition = transform.position });
        }

        private void TrySwitchScheduledDirection()
        {
            if (m_scheduledDirection.HasValue)
                currentDirection = m_scheduledDirection.Value;
            m_scheduledDirection = null;
        }

        private void TryReflectOffWalls()
        {
            var nextLocation = GetNextTileLocation();
            if (this.GetCachedComponentInParent<Field>().IsPassable(nextLocation))
                return;

            currentDirection = ReverseDirection(currentDirection);
            onReverseDirection(currentDirection);
        }

        private Vector2Int GetDirectionVector(Direction dir)
        {
            return m_directionVectors[dir];
        }

        private Vector2Int GetNextTileLocation()
        {
            return m_fieldTransform.snappedLocation + GetDirectionVector(currentDirection);
        }

        private float DistanceUntilNextTile()
        {
            var loc = m_fieldTransform.location;
            var value = IsDirectionVertical(currentDirection) ? loc.y : loc.x;

            var d = 1.0f - Mathf.Repeat(value, 1.0f);
            if (IsDirectionFlipped(currentDirection))
                d = 1.0f - d;

            return d;
        }

        private IEnumerator DashRechargeTimer()
        {
            yield return new WaitForSeconds(m_dashTimeRecharge);
            m_dashCharges++;
            if (m_spawnTail)
            {
                EventBus.Instance.Raise(new GameEvents.OnDashRecharge() { newCount = m_dashCharges });
            }
        }
    }
}
