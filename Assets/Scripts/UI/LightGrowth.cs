using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGrowth : MonoBehaviour {

    public float growthAmount;
    public float cycleSeconds;

    Vector2 initialSize;
    float startTime;

	void Awake () {
        initialSize = transform.localScale;
        startTime = Time.time + Random.Range(0, 2 * Mathf.PI);
    }
	
	void Update () {
        //if (Time.time < startTime) return;
        float scaleAdd = Mathf.Abs(growthAmount * Mathf.Sin((2 * Mathf.PI * (Time.time - startTime)) / cycleSeconds));
        transform.localScale = new Vector2(initialSize.x + scaleAdd, initialSize.y + scaleAdd);		
	}
}
