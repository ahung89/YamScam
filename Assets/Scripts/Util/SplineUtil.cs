using System.Collections.Generic;
using UnityEngine;

public class SplineUtil {

	public static List<SplinePoint> GenerateSpline(List<Vector3> points, int stepsPerCurve = 1, float tension = 1)
    {
        List<SplinePoint> result = new List<SplinePoint>();

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 prev = i == 0 ? points[i] : points[i - 1];
            Vector3 currStart = points[i];
            Vector3 currEnd = points[i + 1];
            Vector3 next = i == points.Count - 2 ? points[i + 1] : points[i + 2];

            for (int step = 0; step < stepsPerCurve; step++)
            {
                float t = (float)step / stepsPerCurve;
                float tSquared = t * t;
                float tCubed = tSquared * t;

                Vector3 interpolatedPoint =
                    (-.5f * tension * tCubed + tension * tSquared - .5f * tension * t) * prev +
                    (1 + .5f * tSquared * (tension - 6) + .5f * tCubed * (4 - tension)) * currStart +
                    (.5f * tCubed * (tension - 4) + .5f * tension * t - (tension - 3) * tSquared) * currEnd +
                    (-.5f * tension * tSquared + .5f * tension * tCubed) * next;

                Vector3 derivative =
                    (-1.5f * tension * tSquared + 2 * tension * t - .5f * tension) * prev +
                    ((tension - 6) * t + 1.5f * tSquared * (4 - tension)) * currStart +
                    (1.5f * (tension - 4) * tSquared + .5f * tension - 2 * (tension - 3) * t) * currEnd +
                    (-tension * t + 1.5f * tension * tSquared) * next;

                result.Add(new SplinePoint(interpolatedPoint, new Vector2(-derivative.y, derivative.x).normalized));
            }
        }

        return result;
    }
}

public struct SplinePoint
{
    public Vector2 point;
    public Vector2 tangent;

    public SplinePoint(Vector2 point, Vector2 tangent)
    {
        this.point = point;
        this.tangent = tangent;
    }
}