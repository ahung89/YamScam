using UnityEngine;

public class Laser : MonoBehaviour {

    public float missedSpeed;
    public float joinSpeed;
    public float timeToImpact;
    public GameObject explosionPrefab;
    public AudioClip laserSound;

    private GameObject target;
    private Vector2 initialPosition;
    private float birthTime;

    private Vector2 missDir;
    private Vector2 targetPosition;
    private Renderer renderer;

    // for determining behavior after joining
    private bool missed = false;
    private bool left = false;
    private bool joined = false;
    private Vector2 joinPosition;
    private Vector2 joinDir;
    private GameObject rightMissile;
    private AudioSource audioSource;

    private Vector2 lastFramePos;

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

    public void PlaySound ()
    {
        audioSource.PlayOneShot(laserSound);
    }

    void InitWithTarget()
    {
        initialPosition = transform.position;
        birthTime = Time.time;
    }

    void Awake()
    {
        renderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
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
            if (!renderer.isVisible)
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

            if (target.tag == TagNames.YAM && !yam.isBad && !yam.destroyed)
            {
                EventBus.PublishEvent(new GoodYamLostEvent());
            }

            if (target.tag == TagNames.SAW)
            {
                target.GetComponent<Saw>().KnockBack();
            }
            if (yam != null)
            {
                yam.Destroy();
            }
        }
        if (other.tag == TagNames.SAW)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            other.GetComponent<Saw>().KnockBack();
        }
    }
}
