using UnityEngine;
using Gamelogic.Extensions;
using System.Collections.Generic;

namespace UnityPrototype
{
    public static class GizmosHelper
    {
        public static void DrawVector(Vector2 pos, Vector2 dir)
        {
            Gizmos.DrawLine(pos, pos + dir);
        }

        public static void DrawCircle(Vector2 origin, float radius, int segments = 16)
        {
            DrawEllipse(origin, Vector2.one * radius, segments);
        }

        public static void DrawEllipse(Vector2 origin, Vector2 axesLength, int segments = 16)
        {
            segments = Mathf.Max(segments, 2);

            var prevDir = Vector2.up;
            var prevPoint = prevDir * axesLength;
            var deltaAngle = 360.0f / segments;

            for (var i = 0; i < segments; i++)
            {
                var dir = prevDir.Rotate(deltaAngle);
                var pos = dir * axesLength;

                Gizmos.DrawLine(origin + prevPoint, origin + pos);

                prevPoint = pos;
                prevDir = dir;
            }
        }

        public static void DrawCurve(IEnumerable<Vector2> points, float pointSize = -1.0f, bool wireframePoint = false)
        {
            Vector2? prevPoint = null;

            foreach (var point in points)
            {
                if (prevPoint.HasValue)
                    Gizmos.DrawLine(prevPoint.Value, point);

                if (pointSize > 0.0f)
                {
                    if (wireframePoint)
                        GizmosHelper.DrawCircle(point, pointSize);
                    else
                        Gizmos.DrawSphere(point, pointSize);
                }

                prevPoint = point;
            }
        }
    }
}
