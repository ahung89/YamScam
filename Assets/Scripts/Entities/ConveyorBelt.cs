using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour {

    public float speed = 2.0f;

    Rigidbody2D rb2d;
    Vector3 originalPos;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        originalPos = transform.position;
    }

    void FixedUpdate ()
    {
        transform.position -= new Vector3(1, 0) * speed * Time.deltaTime;
        rb2d.MovePosition(transform.position + new Vector3(1, 0) * speed * Time.deltaTime);
    }

    void LateUpdate()
    {
        transform.position = originalPos;
        rb2d.position = originalPos;
    }
}
