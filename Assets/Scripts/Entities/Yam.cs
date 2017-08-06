using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yam : MonoBehaviour {
    public GameObject explosion;
    public bool destroyed = false;

    private Rigidbody2D rb2d;
    private GameObject targetBeast;

    // For elevated yams
    private Vector2 elevatedVelocity;

    // For flying yams
    private Vector2 flyVelocity;
    private float timeFlyScale;
    private float flyStartTime;

    private float circleFlightSpeed;

    void Awake ()
    {
    }
	
	void Update ()
    {
        if (flyVelocity != Vector2.zero)
        {
            float newX = flyVelocity.x;
            float newY = flyVelocity.y * Mathf.Sin((Time.time - flyStartTime) * timeFlyScale);
            rb2d.velocity = new Vector2(newX, newY);
        }
        else if (circleFlightSpeed != 0)
        {
            rb2d.velocity = new Vector2(circleFlightSpeed * Mathf.Cos((Time.time - flyStartTime) * 1.5f),
                circleFlightSpeed * Mathf.Sin((Time.time - flyStartTime) * 1.5f));
        }
    }

    public void Elevate(float elevationSpeed)
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(0, elevationSpeed);
        rb2d.gravityScale = 0;
    }

    public void MakeFly(Vector2 flyVelocity, float timeFlyScale)
    {
        rb2d = GetComponent<Rigidbody2D>();
        this.flyVelocity = flyVelocity;
        this.timeFlyScale = timeFlyScale;
        flyStartTime = Time.time;
        rb2d.gravityScale = 0;
    }

    public void MakeFlyCircle(float circleFlightSpeed)
    {
        rb2d = GetComponent<Rigidbody2D>();
        this.circleFlightSpeed = circleFlightSpeed;
        flyStartTime = Time.time;
        rb2d.gravityScale = 0;
    }

    public void SetTargetBeast(GameObject targetBeast)
    {
        this.targetBeast = targetBeast;
    }

    public void HandleBeastKilled(GameObject killedAnimal)
    {
        if (targetBeast == killedAnimal)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    public void Destroy()
    {
        destroyed = true;
        Destroy(gameObject);
    }
}
