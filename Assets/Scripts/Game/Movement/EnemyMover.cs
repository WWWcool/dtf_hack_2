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

        // [SerializeField] private MovementPattern[] m_movementPatterns = new MovementPattern[0];
        [SerializeField] private List<MovementPattern> m_movementPatterns = new List<MovementPattern>();
        [SerializeField] private Animator m_animator;

        private FieldMover m_mover => this.GetCachedComponent<FieldMover>();
        private MovementPattern m_currentPattern => m_movementPatterns[m_patternIndex];
        private Field m_field => this.GetCachedComponentInParent<Field>();
        private FieldTransform m_fieldTransform => this.GetCachedComponent<FieldTransform>();

        private Field.PathData m_pathData = null;
        private System.Action m_onFinished = null;

        private int m_patternIndex = -1;
        private int m_stepsLeft = 0;

        private bool m_enabled => m_movementPatterns != null && m_movementPatterns.Count > 0 && m_patternIndex < m_movementPatterns.Count;

        private void OnEnable()
        {
            m_mover.onTileReached += OnTileReached;
            m_mover.onReverseDirection += OnReverseDirection;

            m_mover.enabled = false;
            m_animator.Play("Idle");
        }

        private void OnDisable()
        {
            m_mover.onTileReached -= OnTileReached;
            m_mover.onReverseDirection -= OnReverseDirection;
        }

        // private void Start()
        // {
        //     NextPattern(true);
        // }

        public void OnTileReached()
        {
            if (!m_enabled)
                return;

            m_stepsLeft--;
            if (m_stepsLeft <= 0)
                NextPattern(false);
        }

        public void NextPattern(bool force)
        {
            UpdateMover();

            if (!m_enabled)
                return;

            // Debug.LogWarning($"NextPattern {m_patternIndex} -> {m_patternIndex + 1} (count = {m_movementPatterns.Count})");
            m_patternIndex++;
            if (!m_enabled)
            {
                FinishPathFollowing();
                return;
            }
            m_stepsLeft = m_currentPattern.length;

            if (force)
                m_mover.currentDirection = m_currentPattern.direction;
            else
            {
                m_mover.ScheduleTurn(m_currentPattern.direction);
                UpdateAnimDir(m_currentPattern.direction);
            }
        }

        private void OnReverseDirection(FieldMover.Direction dir)
        {
            UpdateAnimDir(dir);
        }

        void UpdateAnimDir(FieldMover.Direction dir)
        {
            m_animator.SetBool("isMovingUp", false);
            m_animator.SetBool("isMovingRight", false);
            m_animator.SetBool("isMovingDown", false);
            m_animator.SetBool("isMovingLeft", false);
            switch (dir)
            {
                case FieldMover.Direction.Up:
                    m_animator.SetBool("isMovingUp", true);
                    break;
                case FieldMover.Direction.Right:
                    m_animator.SetBool("isMovingRight", true);
                    break;
                case FieldMover.Direction.Down:
                    m_animator.SetBool("isMovingDown", true);
                    break;
                case FieldMover.Direction.Left:
                    m_animator.SetBool("isMovingLeft", true);
                    break;
            }
        }

        void IdleAnim()
        {
            m_animator.SetBool("isMovingUp", false);
            m_animator.SetBool("isMovingRight", false);
            m_animator.SetBool("isMovingDown", false);
            m_animator.SetBool("isMovingLeft", false);
        }


        public void OnMoveCommand(AIActionMoveContext context)
        {
            Debug.Log("Start Moving");
            var moveType = context.type;
            switch (moveType)
            {
                case ActionMoveType.Line:
                    m_onFinished = context.onFinished;
                    StartFieldMovement();
                    break;
            }
        }

        private void StartFieldMovement()
        {
            m_pathData = RequestNewPath();
            if (m_pathData.waypoints.Count <= 1)
            {
                FinishPathFollowing();
                return;
            }

            UpdateMovementPatterns();
        }

        private void UpdateMovementPatterns()
        {
            m_movementPatterns.Clear();
            m_patternIndex = -1;
            var currentLocation = m_fieldTransform.snappedLocation;
            foreach (var location in m_pathData.waypoints)
            {
                var part = new MovementPattern { direction = GetDirection(currentLocation, location), length = 1 };
                m_movementPatterns.Add(part);
                currentLocation = location;
            }
            NextPattern(false);
        }

        private FieldMover.Direction GetDirection(Vector2Int from, Vector2Int to)
        {
            var delta = to - from;
            Debug.Assert(delta.x == 0 || delta.y == 0);

            if (delta.x > 0)
                return FieldMover.Direction.Right;
            else if (delta.x < 0)
                return FieldMover.Direction.Left;
            else if (delta.y > 0)
                return FieldMover.Direction.Up;
            else if (delta.y < 0)
                return FieldMover.Direction.Down;

            Debug.Assert(false);
            return FieldMover.Direction.Right;
        }

        private Field.PathData RequestNewPath()
        {
            return m_field.FindPathToPlayer(m_fieldTransform);
        }

        private void FinishPathFollowing()
        {
            m_onFinished?.Invoke();
            IdleAnim();
            m_onFinished = null;
            m_pathData = null;

            UpdateMover();

            Debug.Log("Finish Moving");
        }

        private void FixedUpdate()
        {
            UpdateMover();
        }

        private void OnDrawGizmos()
        {
            if (m_pathData == null)
                return;

            var curvePoints = new List<Vector2>(m_pathData.waypoints.Count);
            foreach (var location in m_pathData.waypoints)
                curvePoints.Add(m_field.GetPositionAtLocation(location));

            Gizmos.color = Color.green;
            GizmosHelper.DrawCurve(curvePoints, 0.2f);
        }

        private void UpdateMover()
        {
            m_mover.enabled = m_enabled;
        }
    }
}
