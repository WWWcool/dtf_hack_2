using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UnityPrototype
{
    public class TargetParabolaMover : MonoBehaviour
    {
        [SerializeField] private float m_speed = 10.0f;
        [SerializeField] private UnityEvent m_onTargetReached = null;
        [SerializeField] private float m_heightOffset = 10.0f;

        private Vector2 m_startPosition;

        private Vector2 m_relativeTarget;
        public Vector2 target
        {
            set
            {
                m_startPosition = transform.position;
                m_relativeTarget = value - m_startPosition;
            }
        }

        private float m_targetTime => GetTargetTime(m_relativeTarget);

        private float m_time = 0.0f;

        private void FixedUpdate()
        {
            m_time += Time.fixedDeltaTime;

            var progress = m_time / m_targetTime;
            if (progress > 1.0f)
            {
                m_onTargetReached?.Invoke();
                return;
            }

            var relativePos = GetPosition(progress);
            transform.position = m_startPosition + relativePos;
        }

        private Vector2 GetPosition(float t)
        {
            return GetPosition(t, m_relativeTarget);
        }

        private void CalculateParabola(Vector2 relativeTarget, out float a, out float b)
        {
            var x0 = relativeTarget.x;
            var y0 = relativeTarget.y;
            var h = Mathf.Max(y0, 0.0f) + m_heightOffset;

            b = (4.0f * h - y0) / x0;
            a = (y0 - b * x0) / (x0 * x0);
        }

        private float GetTargetTime(Vector2 relativeTarget)
        {
            var curveLength = GetCurveLength(relativeTarget);
            return curveLength / m_speed;

            // return Mathf.Abs(relativeTarget.x) / m_speed;
        }

        private float GetCurveLength(Vector2 relativeTarget)
        {
            // CalculateParabola(relativeTarget, out var a, out var b);

            // var x0 = relativeTarget.x;

            // return x0 * x0 * (a / 3.0f * x0 + b / 2.0f);

            return Mathf.Abs(relativeTarget.x) + Mathf.Abs(relativeTarget.y); // close enough
        }

        private Vector2 GetPosition(float t, Vector2 relativeTarget)
        {
            CalculateParabola(relativeTarget, out var a, out var b);

            var x = t * relativeTarget.x;
            var y = a * x * x + b * x;

            return new Vector2(x, y);
        }

        private void OnDrawGizmos()
        {
            Vector2 from = m_startPosition;
            Vector2 relativeTarget = m_relativeTarget;

            if (!Application.isPlaying)
            {
                from = (Vector2)transform.position;
                var target = Vector2.zero;
                relativeTarget = target - from;
            }

            DrawGizmos(from, relativeTarget);
        }

        private void DrawGizmos(Vector2 from, Vector2 relativeTarget)
        {
            var time = 0.0f;

            var points = new List<Vector2>();

            var targetTime = GetTargetTime(relativeTarget);

            while (time < targetTime)
            {
                var relativePos = GetPosition(time / targetTime, relativeTarget);
                points.Add(from + relativePos);
                time += 0.05f * targetTime;
            }

            Gizmos.color = Color.cyan;
            GizmosHelper.DrawCurve(points);
        }
    }
}
