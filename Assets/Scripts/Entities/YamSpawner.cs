﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YamSpawner : MonoBehaviour {

    public float yamsPerSecond;
    public float badYamSpawnChance;

    // For elevated yams
    public float elevationSpeed = 0;

    // For flying yams
    public Vector2 flyVelocity;
    public float timeFlyScale;

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
        GameObject spawnedYam = Instantiate(yamToSpawn, transform.position, Quaternion.identity);

        if (elevationSpeed != 0)
        {
            spawnedYam.GetComponent<Yam>().Elevate(elevationSpeed);
        }
        else if (flyVelocity != Vector2.zero)
        {
            spawnedYam.GetComponent<Yam>().MakeFly(flyVelocity, timeFlyScale);
        }

        StartCoroutine(SpawnYam());
    }
}
