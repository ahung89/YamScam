﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yam : MonoBehaviour {
    private Rigidbody2D rb2d;

    // For elevated yams
    private Vector2 elevatedVelocity;

    // For flying yams
    private Vector2 flyVelocity;
    private float timeFlyScale;
    private float flyStartTime;

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
}
