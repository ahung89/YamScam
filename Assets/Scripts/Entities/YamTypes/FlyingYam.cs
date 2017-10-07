using UnityEngine;

public class FlyingYam : Yam {
    public Vector2 speed;
    public float timeFlyScale;
    public float amplitudeScale = 10;

    public float flyStartTime;

    private Vector2 horizontalVec;
    private Vector2 verticalVec;
    private Vector2 lastHorizontal;

    void Start ()
    {
        rb2d = GetComponent<Rigidbody2D>();
        YamSpawner yamSpawner = spawner.GetComponent<YamSpawner>();

        float? inheritedSpeedX = yamSpawner.GetProperty(YamProperty.FlySpeedX);
        float? inheritedSpeedY = yamSpawner.GetProperty(YamProperty.FlySpeedY);
        float? inheritedAmplitude = yamSpawner.GetProperty(YamProperty.SinAmplitude);

        speed = new Vector2(inheritedSpeedX ?? speed.x, inheritedSpeedY ?? speed.y);
        amplitudeScale = inheritedAmplitude ?? amplitudeScale;

        flyStartTime = Time.time;
        rb2d.gravityScale = 0;

        CalculateVectors();
    }

    void CalculateVectors()
    {
        float deg = spawner.transform.eulerAngles.z - 90;
        float z = deg * Mathf.Deg2Rad;

        float xComp = speed.x * Mathf.Cos(z);
        float yComp = speed.x * Mathf.Sin(z);

        horizontalVec = new Vector2(xComp, yComp).normalized;
        verticalVec = RotateVector(horizontalVec, deg > 90 && deg < 270 ? -90 : 90).normalized;

        lastHorizontal = transform.position;
    }

    Vector2 RotateVector(Vector2 vec, float deg)
    {
        float rad = deg * Mathf.Deg2Rad;
        return new Vector2(vec.x * Mathf.Cos(rad) - vec.y * Mathf.Sin(rad), vec.x * Mathf.Sin(rad) + vec.y * Mathf.Cos(rad));
    }

    void FixedUpdate ()
    {
        float verticalWaveOffset = Mathf.Sin((Time.time - flyStartTime) * timeFlyScale) * amplitudeScale;

        Vector2 horizontalOffset = speed.x * Time.deltaTime * horizontalVec;
        Vector2 verticalOffset = (speed.y * Time.deltaTime * verticalWaveOffset) * verticalVec;

        transform.position = lastHorizontal + verticalOffset;
        lastHorizontal += horizontalOffset;
    }
}
