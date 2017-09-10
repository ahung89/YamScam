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
    public GameObject yamPrefab;
    public GameObject targetBeast;
    public List<Sprite> yamSprites;
    public GameObject spawnPosObj;
    public List<SpawnProperty> yamPropertyOverrides;

    private float yamWaitSeconds;
    private GameManager gameManager;
    private bool productionPaused;
    private Vector2 animalReplacementPosition;
    private Vector2 animalOriginalPosition;
    private int replacementDir;
    private HeadMovement hoverScript;
    private bool replacementStarted = false;
    private Vector2 spawnPos;
    private Animator spawnAnimator;

    void Awake()
    {
        gameManager = GameManager.Instance;
        yamsPerSecond = gameManager.difficultyMultiplier;
        animalOriginalPosition = targetBeast.transform.position;
        spawnAnimator = GetComponent<Animator>();
        spawnAnimator.speed = .75f;
        spawnAnimator.enabled = false;
        spawnPos = spawnPosObj.transform.position;

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
    }

    public float? GetProperty(YamProperty prop)
    {
        if (yamPropertyOverrides == null)
        {
            return null;
        }

        foreach (SpawnProperty spawnProp in yamPropertyOverrides)
        {
            if  (spawnProp.property == prop)
            {
                return spawnProp.value;
            }
        }

        return null;
    }

    public void SpawnYam ()
    {
        bool spawnBadYam = Random.Range(0, 100) < badYamSpawnChance;
        GameObject spawnedYam = Instantiate(yamPrefab, spawnPos, Quaternion.identity);
        Yam yam = spawnedYam.GetComponent<Yam>();
        yam.isBad = spawnBadYam;

        if (spawnBadYam)
        {
            DecorateBadYam(yam.gameObject);
        }

        yam.Init(gameObject, targetBeast);
    }

    void DecorateBadYam(GameObject yam)
    {
        yam.GetComponent<SpriteRenderer>().sprite = yamSprites[Random.Range(0, yamSprites.Count)];
    }

    [SubscribeGlobal]
    public void HandleBeastKilled (BeastKilledEvent e)
    {
        if (targetBeast == e.beast)
        {
            productionPaused = true;
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
            Destroy(targetBeast);
            targetBeast = Instantiate(targetBeast, animalReplacementPosition, Quaternion.identity);
            hoverScript = targetBeast.GetComponent<HeadMovement>();
            StartCoroutine(MoveNewAnimalOnScreen());
            targetBeast.GetComponent<Animator>().enabled = true;
            targetBeast.GetComponent<BoxCollider2D>().enabled = true;
            targetBeast.GetComponent<HeadMovement>().enabled = true;
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
            spawnAnimator.enabled = true;
            productionPaused = false;
        }
    }
}
