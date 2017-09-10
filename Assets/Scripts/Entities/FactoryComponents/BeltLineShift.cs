using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeltLineShift : MonoBehaviour {

    public float beltSpeed;

    SpriteRenderer rendera;
    private Vector2 originalOffset;

	void Awake()
    {
        rendera = GetComponent<SpriteRenderer>();
        originalOffset = Vector2.zero;
    }

    void Update()
    {
        rendera.material.SetFloat("_UnscaledTime", Time.time * beltSpeed);
    }
}
