using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Yam")
        {
            Destroy(other.gameObject);
        }
        else if (other.tag == "BadYam")
        {
            HandleAnimalDeath();
        }
    }

    void HandleAnimalDeath()
    {
        GameObject[] yams = GameObject.FindGameObjectsWithTag("Yam");
        GameObject[] badYams = GameObject.FindGameObjectsWithTag("BadYam");
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");

        for (int i = 0; i < yams.Length; i++)
        {
            yams[i].GetComponent<Yam>().HandleBeastKilled(gameObject);
        }

        for (int i = 0; i < badYams.Length; i++)
        {
            badYams[i].GetComponent<Yam>().HandleBeastKilled(gameObject);
        }

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].GetComponent<YamSpawner>().HandleBeastKilled(gameObject);
        }
    }
}
