using System.Collections.Generic;
using UnityEngine;

namespace TomWill
{
    public static class ExtendMath
    {
        public enum PerpendicularType
        {
            ALWAYS_UP, ALWAYS_DOWN, NONE
        }

        private static int numpoints = 20;

        /// <summary>
        /// To get the transform movement like curve with Bezier Quadratic
        /// </summary>
        /// <param name="points"> The list you want to save transform of curve </param>
        /// <param name="startPoint"> Start position you want to move </param>
        /// <param name="destinationPoint"> The destination of your movement </param>
        /// <param name="controlPos"> Transform control to control the curve shape </param>
        public static void BezierCurve(ref List<Vector2> points, Vector2 startPoint, Vector2 destinationPoint,
            Vector2 controlPos, int numpoints)
        {
            float t = 0;
            points.Clear();
            //List<Vector2> points = new List<Vector2>();
            for (int i = 1; i < numpoints + 1; i++)
            {
                t = i / (float)numpoints;
                points.Add(GetBezierCurves(startPoint, destinationPoint, controlPos, t));
            }
        }

        private static Vector2 GetBezierCurves(Vector2 startPoint, Vector2 destinationPoint, Vector2 tempPos, float t)
        {
            return ((1 - t) * ((1 - t) * startPoint + t * tempPos)) +
                   (t * ((1 - t) * tempPos + (t * destinationPoint)));
        }

        /// <summary>
        /// To set the numpoints. The default is 10
        /// </summary>
        /// <param name="_numpoints"></param>
        public static void SetNumPoints(int _numpoints)
        {
            numpoints = _numpoints;
        }

        public static int GetNumPoints()
        {
            return numpoints;
        }

        public static Vector2 GetPerpendicular(Vector2 point1, Vector2 point2, float heightControl = 10f, PerpendicularType type = PerpendicularType.NONE)
        {
            switch (type)
            {
                case PerpendicularType.ALWAYS_UP:
                    heightControl = point1.x > point2.x ? -heightControl : heightControl;
                    break;
                case PerpendicularType.ALWAYS_DOWN:
                    heightControl = point1.x < point2.x ? -heightControl : heightControl;
                    break;
            }

            Vector2 center = (point2 + point1) / 2;
            Vector2 vNormalized = (center - point2).normalized;
            Vector2 vPerpendicular = new Vector2(vNormalized.y, -vNormalized.x).normalized;
            return new Vector2(center.x, center.y) + (vPerpendicular * heightControl);
        }
    }
}