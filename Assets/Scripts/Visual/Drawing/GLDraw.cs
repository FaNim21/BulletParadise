using UnityEngine;

namespace BulletParadise.Visual.Drawing
{
    public sealed class GLDraw
    {
        public static void DrawLine(Vector2 start, Vector2 end, Color color, float width)
        {
            GL.Begin(GL.QUADS);
            GL.Color(color);

            Vector2 perpendicular = (end - start).normalized * width;
            Vector2 normal = new(-perpendicular.y, perpendicular.x);

            GL.Vertex(start - perpendicular - normal);
            GL.Vertex(start - perpendicular + normal);
            GL.Vertex(end + perpendicular + normal);
            GL.Vertex(end + perpendicular - normal);

            GL.End();
        }

        public static void DrawRay(Vector3 position, Vector3 direction, Color color, float width = 0.02f)
        {
            Vector3 endPosition = position + direction;

            DrawLine(position, endPosition, color, width);
        }

        public static void DrawBox(Vector2 center, Vector2 size, Color color, float width = 0.01f)
        {
            float halfWidth = size.x / 2f;
            float halfHeight = size.y / 2f;

            Vector2 topLeftCorner = new(center.x - halfWidth, center.y - halfHeight);
            Vector2 bottomRightCorner = new(center.x + halfWidth, center.y + halfHeight);

            Rect box = new(topLeftCorner.x, topLeftCorner.y, bottomRightCorner.x - topLeftCorner.x, bottomRightCorner.y - topLeftCorner.y);
            DrawBox(box, color, width);
        }
        public static void DrawBox(Rect box, Color color, float width)
        {
            Vector2 v1 = new(box.xMin, box.yMin);
            Vector2 v2 = new(box.xMax, box.yMin);
            Vector2 v3 = new(box.xMax, box.yMax);
            Vector2 v4 = new(box.xMin, box.yMax);

            DrawLine(v1, v2, color, width);
            DrawLine(v2, v3, color, width);
            DrawLine(v3, v4, color, width);
            DrawLine(v4, v1, color, width);
        }

        public static void DrawCircle(Vector2 center, float radius, Color color, float width = 0.01f, int segments = 360)
        {
            float angleStep = 360f / segments;

            Vector2 previousPoint = center + new Vector2(Mathf.Cos(0), Mathf.Sin(0)) * radius;

            for (float angle = angleStep; angle <= 360; angle += angleStep)
            {
                Vector2 nextPoint = center + new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;
                DrawLine(previousPoint, nextPoint, color, width);
                previousPoint = nextPoint;
            }
        }

        public static void DrawCurvedLine(Vector2 startPoint, Vector2 endPoint, Color color, float width, float curveIntensity)
        {
            float midX = (startPoint.x + endPoint.x) / 2f;
            float midY = (startPoint.y + endPoint.y) / 2f;

            Vector2 midPoint = new(midX, midY);

            Vector2 direction = (endPoint - startPoint).normalized;
            Vector2 perpendicular = new(-direction.y, direction.x);

            float distance = Vector2.Distance(startPoint, endPoint) * curveIntensity;

            Vector2 controlPoint = midPoint + (perpendicular * distance);

            DrawBezier(startPoint, controlPoint, endPoint, controlPoint, color, width);
        }

        public static void DrawBezier(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, Color color, float width)
        {
            float length = BezierCurveLength(start, startTangent, end, endTangent);
            int segments = Mathf.CeilToInt(length * 2);
            segments = Mathf.Clamp(segments, 20, int.MaxValue);

            for (int i = 0; i <= segments; i++)
            {
                float t = i / (float)segments;
                Vector2 point = CubicBezierPoint(start, startTangent, end, endTangent, t);

                if (i > 0)
                {
                    Vector2 prevPoint = CubicBezierPoint(start, startTangent, end, endTangent, (i - 1) / (float)segments);
                    DrawLine(prevPoint, point, color, width);
                }
            }
        }
        private static float BezierCurveLength(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent)
        {
            int steps = 50;
            float length = 0;
            Vector2 prevPoint = start;

            for (int i = 1; i <= steps; i++)
            {
                float t = i / (float)steps;
                Vector2 point = CubicBezierPoint(start, startTangent, end, endTangent, t);

                length += Vector2.Distance(prevPoint, point);
                prevPoint = point;
            }

            return length;
        }
        private static Vector2 CubicBezierPoint(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            float uuu = uu * u;
            float ttt = tt * t;

            Vector2 point = uuu * start;
            point += 3 * uu * t * startTangent;
            point += 3 * u * tt * endTangent;
            point += ttt * end;

            return point;
        }
    }
}