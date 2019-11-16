using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityPrototype
{
    public class PlayerInputListener : MonoBehaviour
    {
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
        }

        private void OnDisable()
        {
            EventBus.Instance.RemoveListener<InputEvents.SwipeDetected>(OnSwipeDetected);
            EventBus.Instance.RemoveListener<InputEvents.TapDetected>(OnTapDetected);
        }

        private void OnSwipeDetected(InputEvents.SwipeDetected e)
        {
            this.GetCachedComponent<FieldMover>().ScheduleTurn(GetTurnDirection(e.direction));
        }

        private void OnTapDetected(InputEvents.TapDetected e)
        {
            this.GetCachedComponent<FieldMover>().TryDash();
        }

        private FieldMover.Direction GetTurnDirection(InputEvents.SwipeDirection dir)
        {
            return m_directionsMap[dir];
        }
    }
}
