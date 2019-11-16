using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityPrototype
{
    public class SwipeArea : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
    {
        [SerializeField] private float m_distanceThreshold = 0.0f;

        private static readonly InputEvents.SwipeDirection[] m_directions = new InputEvents.SwipeDirection[] {
            InputEvents.SwipeDirection.Right,
            InputEvents.SwipeDirection.Up,
            InputEvents.SwipeDirection.Left,
            InputEvents.SwipeDirection.Down,
        };

        private Vector2 m_startPoint;
        private bool m_inputInProgress = false;
        private bool m_canClick = true;

        private Vector2 GetPosition(PointerEventData eventData)
        {
            return eventData.position;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_startPoint = GetPosition(eventData);
            m_inputInProgress = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            m_canClick = false;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var endPoint = GetPosition(eventData);
            m_inputInProgress = false;
            m_canClick = true;
            var delta = endPoint - m_startPoint;
            if (delta.sqrMagnitude < m_distanceThreshold * m_distanceThreshold)
                return;

            var direction = CalcualteDirection(delta);

            EventBus.Instance.Raise(new InputEvents.SwipeDetected { direction = direction });
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_canClick)
            {
                EventBus.Instance.Raise(new InputEvents.TapDetected());
            }
        }

        private InputEvents.SwipeDirection CalcualteDirection(Vector2 delta)
        {
            var sections = 4;
            var sectionAngle = 360.0f / sections;

            var angle = Vector2.SignedAngle(Vector2.right, delta);
            if (angle < 0.0f)
                angle += 360.0f;
            var directionIndex = Mathf.RoundToInt(angle / sectionAngle);

            directionIndex = directionIndex % sections;

            return m_directions[directionIndex];
        }
    }
}
