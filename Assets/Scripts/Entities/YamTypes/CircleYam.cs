using UnityEngine;

public class CircleYam : Yam {

    public float circleFlightSpeed;
    private float flyStartTime;

    void Start ()
    {
        rb2d = GetComponent<Rigidbody2D>();
        flyStartTime = Time.time;
        rb2d.gravityScale = 0;
    }

    private void Update ()
    {
        rb2d.velocity = new Vector2(circleFlightSpeed * Mathf.Cos((Time.time - flyStartTime) * 1.5f),
            circleFlightSpeed * Mathf.Sin((Time.time - flyStartTime) * 1.5f));
    }
}
