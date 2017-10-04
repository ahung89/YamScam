using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class Mushroom : MonoBehaviour {
    public GameObject target;

    public float height = 1.5f;
    private static int numDashes = 30;

    private float adjustedHeight;
    private float trajectoryDuration;

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
        return new Vector2(velX, velY);
    }

    private void OnDrawGizmos ()
    {
        bool selected = Selection.Contains(gameObject);
        Color col = selected ? Color.green : Color.grey;
        col.a = selected ? 1 : .5f;

        Vector2 launchVelocity = CalculateLaunchVelocity();
        Vector3 prevPoint = transform.position;

        for (int i = 1; i < numDashes; i++)
        {
            float t = i / (float)numDashes * trajectoryDuration;
            Vector2 displacement = .5f * Physics.gravity.y * Vector2.up * t * t + launchVelocity * t;
            Vector3 point = (Vector2)transform.position + displacement;

            if (i % 2 != 0)
            {
                Debug.DrawLine(prevPoint, point, col);
            }

            prevPoint = point;
        }
    }
}
