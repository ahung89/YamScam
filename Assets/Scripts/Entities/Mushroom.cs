using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

[ExecuteInEditMode]
public class Mushroom : MonoBehaviour {
    public GameObject target;

    [Range(-1.5f, 1.5f)]
    public float height = 1.5f;
    public float arrowLength = .1f;
    public float arrowCountMultiplier = 5;

    private float adjustedHeight;
    private float trajectoryDuration;
    private float xDistance;
    private float a;

    private void OnTriggerEnter2D (Collider2D collider)
    {
        if (!target)
        {
            return;
        }

        collider.transform.position = transform.position;
        collider.GetComponent<Rigidbody2D>().velocity = CalculateLaunchVelocity();
    }

    Vector2 CalculateLaunchVelocity ()
    {
        Vector2 transformedPoint = transform.InverseTransformPoint(target.transform.position);
        adjustedHeight = Mathf.Max(0, transformedPoint.y) + height;

        float displacementY = transformedPoint.y;
        float displacementX = transformedPoint.x;

        trajectoryDuration = Mathf.Sqrt(-2 * adjustedHeight / Physics.gravity.y) + Mathf.Sqrt(2 * (displacementY - adjustedHeight) / Physics.gravity.y);

        float velY = Mathf.Sqrt(-2 * Physics.gravity.y * adjustedHeight);
        float velX = displacementX / (Mathf.Sqrt(-2 * adjustedHeight / Physics.gravity.y) + Mathf.Sqrt(2 * (displacementY - adjustedHeight) / Physics.gravity.y));

        float timeToVertex = -velY / Physics.gravity.y;
        xDistance = velX * timeToVertex;

        a = (transformedPoint.y - adjustedHeight) / Mathf.Pow(transformedPoint.x - xDistance, 2);

        return new Vector2(velX, velY);
    }

    float CalculateHeight(float x)
    {
        return a * Mathf.Pow(x - xDistance, 2) + adjustedHeight;
    }

    float CalculateSlope(float x)
    {
        return 2 * a * (x - xDistance);
    }

    Vector2 RotateVector(Vector2 vec, float angle)
    {
        return new Vector2(vec.x * Mathf.Cos(angle) - vec.y * Mathf.Sin(angle), vec.x * Mathf.Sin(angle) + vec.y * Mathf.Cos(angle));
    }

#if UNITY_EDITOR
    private void OnDrawGizmos ()
    {
        bool selected = Selection.Contains(gameObject);
        Color col = selected ? Color.green : Color.grey;
        col.a = selected ? 1 : .5f;
        Gizmos.color = col;

        float sign = Mathf.Sign(target.transform.position.x - transform.position.x);

        Vector2 launchVelocity = CalculateLaunchVelocity(); 
        Vector3 prevPoint = transform.position;

        int numArrows = (int)(arrowCountMultiplier * trajectoryDuration);
        float increment = (target.transform.position.x - transform.position.x) / numArrows;

        for (int i = 1; i < numArrows; i++)
        {
            float x = increment * i;
            float y = transform.position.y + CalculateHeight(increment * i);
            float slope = CalculateSlope(x);
            Vector2 tangent = sign * new Vector2(1, slope);

            Vector2 point = new Vector2(transform.position.x + x, transform.position.y + CalculateHeight(x));
            Vector2 angle1 = RotateVector(tangent, -20 * Mathf.Deg2Rad).normalized * arrowLength;
            Vector2 angle2 = RotateVector(tangent, 20 * Mathf.Deg2Rad).normalized * arrowLength;

            if (i % 2 != 0)
            {
                Gizmos.DrawLine(point - angle1, point);
                Gizmos.DrawLine(point - angle2, point);
            }

            prevPoint = point;
            }
        }
#endif
}
