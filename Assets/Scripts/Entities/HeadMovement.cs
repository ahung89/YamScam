using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMovement : MonoBehaviour {

    public float movementAmount;
    public float secondsPerCycle;

    Vector2 initialPos;
    float roundStart;

    void Awake()
    {
        initialPos = transform.position;
        roundStart = Time.time + Random.Range(0, Mathf.PI * 2);
    }

	void Update () {
        transform.position = new Vector2(initialPos.x, initialPos.y +
            (movementAmount * Mathf.Sin(Mathf.PI * 2 * ((Time.time - roundStart) / secondsPerCycle))));
	}

    public void UpdateInitialPos(float newX)
    {
        initialPos = new Vector2(newX, initialPos.y);
    }
}
