﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour {

    public float radius;

    Vector2 rootPos;

	void Start () {
        rootPos = transform.position;		
	}
	
	void Update () {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 vec = mousePos - rootPos;
        float dist = Vector2.Distance(transform.parent.position, mousePos);

        if (dist < radius)
        {
            transform.position = mousePos;
        }
        else
        {
            transform.position = rootPos + radius * vec.normalized;
        }
	}
}