using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class PlayerInputListener : MonoBehaviour
    {
        [SerializeField] private Animator m_animator;
        private static readonly Dictionary<InputEvents.SwipeDirection, FieldMover.Direction> m_directionsMap = new Dictionary<InputEvents.SwipeDirection, FieldMover.Direction> {
            {InputEvents.SwipeDirection.Left, FieldMover.Direction.Left},
            {InputEvents.SwipeDirection.Right, FieldMover.Direction.Right},
            {InputEvents.SwipeDirection.Up, FieldMover.Direction.Up},
            {InputEvents.SwipeDirection.Down, FieldMover.Direction.Down},
        };

        private void OnEnable()
        {
            EventBus.Instance.AddListener<InputEvents.SwipeDetected>(OnSwipeDetected);
            EventBus.Instance.AddListener<InputEvents.TapDetected>(OnTapDetected);
            m_animator.Play("MoveRight");
            m_animator.SetBool("isMovingRight", true);
            this.GetCachedComponent<FieldMover>().onReverseDirection += OnReverseDirection;
        }

        private void OnDisable()
        {
            EventBus.Instance.RemoveListener<InputEvents.SwipeDetected>(OnSwipeDetected);
            EventBus.Instance.RemoveListener<InputEvents.TapDetected>(OnTapDetected);
        }

        private void OnSwipeDetected(InputEvents.SwipeDetected e)
        {
            var dir = GetTurnDirection(e.direction);
            this.GetCachedComponent<FieldMover>().ScheduleTurn(dir);
            UpdateAnimDir(dir);
        }

        private void OnTapDetected(InputEvents.TapDetected e)
        {
            this.GetCachedComponent<FieldMover>().TryDash();
        }

        private void OnReverseDirection(FieldMover.Direction dir)
        {
            UpdateAnimDir(dir);
        }

        private FieldMover.Direction GetTurnDirection(InputEvents.SwipeDirection dir)
        {
            return m_directionsMap[dir];
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
    }
}
