using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YamSpawner : MonoBehaviour {

    public float yamsPerSecond;
    public float badYamSpawnChance;
    public float animalReplacementDistance;
    public float animalReplacementSpeed;
    public float replacementWaitTime;

    public Sprite originalFrame;

    // For elevated yams
    public float elevationSpeed = 0;

    // For flying yams
    public Vector2 flyVelocity;
    public float timeFlyScale;

    // For circle yams
    public float circleFlightSpeed;

    public GameObject yam;
    public GameObject fuckedYam;
    public GameObject targetBeast;

    public List<Sprite> easyFuckedYams;
    public List<Sprite> mediumFuckedYams;
    public List<Sprite> difficultFuckedYams;

    private float yamWaitSeconds;
    private GameManager gameManager;
    private bool productionPaused;

    Vector2 animalReplacementPosition;
    Vector2 animalOriginalPosition;
    int replacementDir;
    HeadMovement hoverScript;
    bool replacementStarted = false;

    Vector2 spawnPos;

    Animator spawnAnimator;

    void Awake()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        yamsPerSecond = gameManager.difficultyMultiplier;
        animalOriginalPosition = targetBeast.transform.position;
        spawnAnimator = GetComponent<Animator>();
        spawnAnimator.speed = .75f;
        spawnAnimator.enabled = false;
        spawnPos = transform.Find("SpawnPos").transform.position;

        if (Camera.main.WorldToViewportPoint(targetBeast.transform.position).x < .5f)
        {
            animalReplacementPosition = targetBeast.transform.position - new Vector3(animalReplacementDistance, 0, 0);
            replacementDir = -1;
        }
        else
        {
            animalReplacementPosition = targetBeast.transform.position + new Vector3(animalReplacementDistance, 0, 0);
            replacementDir = 1;
        }
        hoverScript = targetBeast.GetComponent<HeadMovement>();
    }

    [SubscribeGlobal]
    public void HandleGameStart(GameStartedEvent e)
    {
        yamWaitSeconds = 1 / yamsPerSecond;
        spawnAnimator.enabled = true;
        //StartCoroutine(SpawnYam());
    }

    public void SpawnYam ()
    {
        //yield return new WaitForSeconds(yamWaitSeconds);
        GameObject yamToSpawn = Random.Range(0, 100) < badYamSpawnChance ? fuckedYam : yam;
        GameObject spawnedYam = Instantiate(yamToSpawn, spawnPos, Quaternion.identity);
        Yam datYam = spawnedYam.GetComponent<Yam>();

        if (yamToSpawn == fuckedYam)
        {
            DecorateFuckedYam(datYam.gameObject);
        }

        if (elevationSpeed != 0)
        {
            datYam.Elevate(elevationSpeed);
        }
        else if (flyVelocity != Vector2.zero)
        {
            datYam.MakeFly(flyVelocity, timeFlyScale);
        }
        else if (circleFlightSpeed != 0)
        {
            datYam.MakeFlyCircle(circleFlightSpeed);
        }

        datYam.SetTargetBeast(targetBeast);

        //StartCoroutine(SpawnYam());
    }

    void DecorateFuckedYam(GameObject yizzam)
    {
        List<Sprite> yamSprites;
        
        if (gameManager.difficulty == 0)
        {
            yamSprites = easyFuckedYams;
        }
        else if (gameManager.difficulty == 1)
        {
            yamSprites = mediumFuckedYams;
        }
        else
        {
            yamSprites = difficultFuckedYams;
        }

        yizzam.GetComponent<SpriteRenderer>().sprite = yamSprites[Random.Range(0, yamSprites.Count)];
    }

    public void HandleBeastKilled (GameObject killedAnimal)
    {
        if (targetBeast == killedAnimal)
        {
            productionPaused = true;
            //StopAllCoroutines();
            spawnAnimator.enabled = false;
            StartCoroutine(MoveKilledAnimalOffscreen());
            GetComponent<SpriteRenderer>().sprite = originalFrame;
        }
    }

    IEnumerator MoveKilledAnimalOffscreen()
    {
        if (!replacementStarted)
        {
            replacementStarted = true;
            yield return new WaitForSeconds(replacementWaitTime);
        }
        targetBeast.transform.position = Vector2.MoveTowards(targetBeast.transform.position, animalReplacementPosition, Time.deltaTime * animalReplacementSpeed);
        hoverScript.UpdateInitialPos(targetBeast.transform.position.x);
        yield return null;
        if (Vector2.Distance(targetBeast.transform.position, animalReplacementPosition) > .1f)
        {
            StartCoroutine(MoveKilledAnimalOffscreen());
        }
        else
        {
            //replace animal
            Destroy(targetBeast);
            targetBeast = Instantiate(targetBeast, animalReplacementPosition, Quaternion.identity);
            hoverScript = targetBeast.GetComponent<HeadMovement>();
            StartCoroutine(MoveNewAnimalOnScreen());
            targetBeast.GetComponent<Animator>().enabled = true;
            targetBeast.GetComponent<BoxCollider2D>().enabled = true;
            targetBeast.GetComponent<HeadMovement>().enabled = true;
            //targetBeast.GetComponent<>
            //set anims
            //animate back to original position
        }
    }

    IEnumerator MoveNewAnimalOnScreen()
    {
        targetBeast.transform.position = Vector2.MoveTowards(targetBeast.transform.position, animalOriginalPosition, Time.deltaTime * animalReplacementSpeed);
        hoverScript.UpdateInitialPos(targetBeast.transform.position.x);
        yield return null;
        if (Vector2.Distance(targetBeast.transform.position, animalOriginalPosition) > .1f)
        {
            StartCoroutine(MoveNewAnimalOnScreen());
        }
        else
        {
            //StartCoroutine(SpawnYam());
            spawnAnimator.enabled = true;
            productionPaused = false;
        }
    }
}
