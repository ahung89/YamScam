using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public float missedSpeed;
    public float joinSpeed;
    public float timeToImpact;
    public GameObject explosionPrefab;
    GameObject target;
    Vector2 initialPosition;
    float birthTime;

    Vector2 missDir;
    Vector2 targetPosition;
    Renderer renderera;

    // for determining behavior after joining
    bool missed = false;
    bool left = false;
    bool joined = false;
    Vector2 joinPosition;
    Vector2 joinDir;
    GameObject rightMissile;

    Vector2 lastFramePos;

    public void SetTarget(GameObject target, Vector2 joinPosition, bool left, GameObject rightMissile = null)
    {
        this.rightMissile = rightMissile;
        this.left = left;
        missed = false;
        this.target = target;
        targetPosition = target.transform.position;

        this.joinPosition = joinPosition;
        joinDir = (joinPosition - (Vector2)transform.position).normalized;
    }

    public void SetMissDir(Vector2 missDir, Vector2 joinPosition, bool left, GameObject rightMissile = null)
    {
        this.rightMissile = rightMissile;
        this.left = left;
        missed = true;
        this.missDir = missDir.normalized;

        this.joinPosition = joinPosition;
        joinDir = (joinPosition - (Vector2)transform.position).normalized;
    }

    void InitWithTarget()
    {
        initialPosition = transform.position;
        birthTime = Time.time;
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
        if (lastFramePos == (Vector2)transform.position && target == null)
        {
            Destroy(gameObject);
        }

        lastFramePos = transform.position;
        if (!joined)
        {
            transform.position += (Vector3)(joinSpeed * joinDir);
            if (Vector2.Distance(transform.position, joinPosition) < .4f)
            {
                if (!missed)
                {
                    InitWithTarget();
                }
                joined = true;
            }
            return;
        }

        if (left && rightMissile != null)
        {
            transform.position = rightMissile.transform.position;
        }

        if (target != null)
        {
            if (initialPosition == (Vector2)target.transform.position)
            {
                Destroy(gameObject);
            }
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
            Yam yam = other.GetComponent<Yam>();

            if (target.tag == "Yam" && !yam.destroyed)
            {
                GameObject.Find("Game Manager").GetComponent<GameManager>().IncrementGoodYamLost();
                GameObject.Find("Lost Yams").GetComponent<IconStrip>().Mark();
                yam.targetBeast.GetComponent<Animal>().Anger();
                yam.targetBeast.GetComponent<AudioSource>().Play();
            }

            if (target.tag == "Saw")
            {
                target.GetComponent<Saw>().KnockBack();
            }
            if (yam != null)
            {
                yam.Destroy();
            }
        }
        if (other.tag == "Saw")
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            other.GetComponent<Saw>().KnockBack();
        }
    }
}
