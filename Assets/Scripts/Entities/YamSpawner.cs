using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YamSpawner : MonoBehaviour {

    public float yamsPerSecond;
    public float badYamSpawnChance;
    public GameObject yam;
    public GameObject fuckedYam;

    private float yamWaitSeconds;

    void Awake()
    {
        yamWaitSeconds = 1 / yamsPerSecond;
        StartCoroutine(SpawnYam());
    }

    IEnumerator SpawnYam()
    {
        yield return new WaitForSeconds(yamWaitSeconds);
        GameObject yamToSpawn = Random.Range(0, 100) < badYamSpawnChance ? fuckedYam : yam;
        Instantiate(yamToSpawn, transform.position, Quaternion.identity);
        StartCoroutine(SpawnYam());
    }
}
