using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public float missedSpeed;
    public float timeToImpact;
    public GameObject explosionPrefab;
    GameObject target;
    Vector3 initialPosition;
    float birthTime;

    Vector2 missDir;
    Renderer renderera;

    public void SetTarget(GameObject target)
    {
        initialPosition = transform.position;
        this.target = target;
        birthTime = Time.time;
    }

    public void SetMissDir(Vector2 missDir)
    {
        this.missDir = missDir;
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
            transform.position = Vector2.Lerp(initialPosition, target.transform.position, (Time.time - birthTime) /timeToImpact);
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
        }
    }
}
