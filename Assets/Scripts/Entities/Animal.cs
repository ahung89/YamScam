using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {

    public Sprite waitFrame;
    public Sprite munchFrame;
    public float munchTime;

    SpriteRenderer rendera;

    void Awake()
    {
        rendera = GetComponent<SpriteRenderer>();
    }

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Yam")
        {
            Destroy(other.gameObject);
            if (rendera.sprite != munchFrame)
            {
                rendera.sprite = munchFrame;
                Invoke("Unmunch", munchTime);
            }
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
        GameObject.Find("Lives").GetComponent<IconStrip>().Mark();

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

    void Unmunch()
    {
        rendera.sprite = waitFrame;
    }
}
