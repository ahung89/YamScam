using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour {

    public Vector2 dir;
    public float speed;
    public float knockbackSpeed;
    public float knockbackDuration;

    bool knockbackInProgress;
    float knockbackStartTime;

    void Update()
    {
        if (!knockbackInProgress)
        {
            transform.position = transform.position + speed * (Vector3)dir * Time.deltaTime;
        }
        else
        {
            if (Time.time > knockbackStartTime + knockbackDuration)
            {
                knockbackInProgress = false;
            }
            else
            {
                transform.position = transform.position - knockbackSpeed * (Vector3)dir * Time.deltaTime;
            }
        }

        Vector2 ll = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 ur = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        transform.position = new Vector2(Mathf.Clamp(transform.position.x, ll.x, ur.x), Mathf.Clamp(transform.position.y, ll.y, ur.y));
    }

    public void KnockBack()
    {
        knockbackInProgress = true;
        knockbackStartTime = Time.time;
    }
}
