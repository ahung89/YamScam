using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour {

    public GameObject target;
    public GameObject pseudoGizmo;
    public GameObject testProjectile;

    public float launchSpeed;
    public float heightModifier = .7f;

    public float initialVelocityY;

    private Vector2 transformedTarget;
    private float midpointX;

    private float a;
    private float k;
    private float startTime;

    private void Awake ()
    {
        startTime = Time.time;
    }

    private void Update ()
    {
        if (target == null)
        {
            return;
        }

        transformedTarget = transform.InverseTransformPoint(target.transform.position);
        float velX = GetVelocityX();
        CalculateMidpoint();
        CalculateArcHeight();

        Vector2 vertex = new Vector2(transform.position.x + midpointX, transform.position.y + k);
        pseudoGizmo.transform.position = vertex;

        float t = Time.time - startTime;
        float x = t * velX;
        Vector2 offset = new Vector2(x, SolveCurrentHeight(x));

        testProjectile.transform.position = (Vector2)transform.position + offset;

        if (x > transformedTarget.x)
        {
            startTime = Time.time;
        }
    }

    void CalculateMidpoint()
    {
        if (transformedTarget.y == 0)
        {
            midpointX = transformedTarget.x / 2;
            return;
        }
        midpointX = transformedTarget.y <= 0 ?
            Mathf.Min(.95f, heightModifier) * transformedTarget.x / 2 : Mathf.Max(.05f, 1 - heightModifier) * transformedTarget.x / 2 + transformedTarget.x / 2;
    }

    void CalculateArcHeight()
    {
        float q = transformedTarget.x;
        float r = transformedTarget.y;

        float h = midpointX;
        float n = q - h;

        a = -r / (h * h - n * n);

        k = h == n ? Mathf.Clamp(transformedTarget.x * .75f, .5f, 1.5f) : r - a * n * n;
    }

    float SolveCurrentHeight(float x)
    {
        return a * Mathf.Pow((x - midpointX), 2) + k;
    }

    float GetVelocityX()
    {
        return launchSpeed * Mathf.Cos(Mathf.Atan2(transformedTarget.y, transformedTarget.x));
    }
}
