using System.Collections;
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

    // For circle yams
    public float circleFlightSpeed;

    public GameObject yam;
    public GameObject fuckedYam;
    public GameObject targetBeast;

    private float yamWaitSeconds;

    void Awake()
    {
        yamWaitSeconds = 1 / yamsPerSecond;
        StartCoroutine(SpawnYam());
    }

    IEnumerator SpawnYam ()
    {
        yield return new WaitForSeconds(yamWaitSeconds);
        GameObject yamToSpawn = Random.Range(0, 100) < badYamSpawnChance ? fuckedYam : yam;
        GameObject spawnedYam = Instantiate(yamToSpawn, transform.position, Quaternion.identity);
        Yam datYam = spawnedYam.GetComponent<Yam>();

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

        StartCoroutine(SpawnYam());
    }

    public void HandleBeastKilled (GameObject killedAnimal)
    {
        if (targetBeast == killedAnimal)
        {
            Destroy(gameObject);
        }
    }
}
