using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour {

    public Vector2 dir;
    public float speed;
    public float knockbackSpeed;
    public float knockbackDuration;
    public float rotationSpeed;

    bool knockbackInProgress;
    float knockbackStartTime;
    bool gameStarted = false;
    float accumulatedRotationZ = 0;

    GameObject sawParent;

    void Awake()
    {
        speed *= GameManager.Instance.difficultyMultiplier;
        sawParent = transform.parent.gameObject;
    }

    [SubscribeGlobal]
    public void Init(GameStartedEvent e)
    {
        gameStarted = true;
    }

    void Update()
    {
        accumulatedRotationZ += Time.deltaTime * rotationSpeed * sawParent.transform.localScale.x * -1;
        transform.rotation = Quaternion.Euler(new Vector3(1, 1, accumulatedRotationZ));

        if (!gameStarted)
        {
            return;
        }

        if (!knockbackInProgress)
        {
            sawParent.transform.position = sawParent.transform.position + speed * (Vector3)dir * Time.deltaTime;
        }
        else
        {
            if (Time.time > knockbackStartTime + knockbackDuration)
            {
                knockbackInProgress = false;
            }
            else
            {
                sawParent.transform.position = sawParent.transform.position - knockbackSpeed * (Vector3)dir * Time.deltaTime;
            }
        }

        Vector2 ll = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 ur = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //sawParent.transform.position = new Vector2(Mathf.Clamp(sawParent.transform.position.x, ll.x, ur.x), Mathf.Clamp(sawParent.transform.position.y, ll.y, ur.y));
    }

    public void KnockBack()
    {
        knockbackInProgress = true;
        knockbackStartTime = Time.time;
    }
}
