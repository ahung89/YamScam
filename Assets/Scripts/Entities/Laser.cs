using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public float missedSpeed;
    public float timeToImpact;
    public GameObject explosionPrefab;
    GameObject target;
    Vector2 initialPosition;
    float birthTime;

    Vector2 missDir;
    Vector2 targetPosition;
    Renderer renderera;

    public void SetTarget(GameObject target)
    {
        initialPosition = transform.position;
        this.target = target;
        targetPosition = target.transform.position;
        birthTime = Time.time;
    }

    public void SetMissDir(Vector2 missDir)
    {
        this.missDir = missDir.normalized;
    }

    void Awake()
    {
        renderera = GetComponent<Renderer>();
    }

    void OnLevelWasLoaded()
    {
        Destroy(gameObject);
    }

	void Update () {
		if (target != null)
        {
            if (initialPosition == targetPosition)
            {
                Destroy(gameObject);
            }
            transform.position = Vector2.Lerp(initialPosition, targetPosition, (Time.time - birthTime) /timeToImpact);
        }
        else
        {
            transform.position += (Vector3)(missedSpeed * missDir);
            if (!renderera.isVisible)
            {
                Destroy(gameObject);
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == target)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            other.GetComponent<Yam>().Destroy();

            if (target.tag == "Yam")
            {
                GameObject.Find("Lost Yams").GetComponent<IconStrip>().Mark();
            }
        }
    }
}
