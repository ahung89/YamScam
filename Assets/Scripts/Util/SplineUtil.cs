using System.Collections.Generic;
using UnityEngine;

public class SplineUtil {

	public static List<Vector3> GenerateSpline(List<Vector3> points, int stepsPerCurve, float tension = 1)
    {
        List<Vector3> result = new List<Vector3>();

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

                Vector3 interpolatedPoint = .5f *
                    ((-tension * tCubed + 2 * tension * tSquared - tension * t) * prev +
                    (2 + tSquared * (tension - 6) + tCubed * (4 - tension)) * currStart +
                    (tCubed * (tension - 4) + tension * t - 2 * (tension - 3) * tSquared) * currEnd +
                    (-tension * tSquared + tension * tCubed) * next);

                result.Add(interpolatedPoint);
            }
        }

        return result;
    }
}
