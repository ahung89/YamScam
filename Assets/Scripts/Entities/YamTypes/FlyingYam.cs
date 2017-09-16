using UnityEngine;

public class FlyingYam : Yam {
    public Vector2 speed;
    public float timeFlyScale;

    public float flyStartTime;

    private float horizontalSpeedInternal;

    void Start ()
    {
        float? inheritedSpeedX = spawner.GetComponent<YamSpawner>().GetProperty(YamProperty.FlySpeedX);
        float? inheritedSpeedY = spawner.GetComponent<YamSpawner>().GetProperty(YamProperty.FlySpeedY);
        speed = new Vector2(inheritedSpeedX ?? speed.x, inheritedSpeedY ?? speed.y);

        rb2d = GetComponent<Rigidbody2D>();
        flyStartTime = Time.time;
        rb2d.gravityScale = 0;
        horizontalSpeedInternal = spawner.transform.eulerAngles.z < 180 ? speed.x : -speed.x;
    }

    void Update ()
    {
        float newY = speed.y * Mathf.Sin((Time.time - flyStartTime) * timeFlyScale);
        rb2d.velocity = new Vector2(horizontalSpeedInternal, newY);
    }
}
