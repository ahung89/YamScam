using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour {

    public float radius;

    Vector2 rootLocalPos;

	void Start () {
        rootLocalPos = transform.localPosition;
    }
	
	void Update () {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 vec = mousePos - (Vector2)transform.parent.position;
        float dist = Vector2.Distance(transform.parent.position, mousePos);

        transform.localPosition = rootLocalPos + radius * vec.normalized;
	}
}
