using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingYam : MonoBehaviour {

    public float elevationSpeed;

    private Rigidbody2D rb2d;

    private void Awake ()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector3(0, elevationSpeed, 0);
    }

    void Update () {
	}
}
