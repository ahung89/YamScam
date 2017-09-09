using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour {

    public Sprite waitFrame;
    public Sprite munchFrame;
    public Sprite angryFrame;
    public Sprite deadFrame;
    public float munchTime;
    public float angryTime;
    public float deathScreenShakeDuration = .3f;

    SpriteRenderer rendera;
    GameManager manager;
    Animator animator;

    void Awake()
    {
        rendera = GetComponent<SpriteRenderer>();
        manager = GameManager.Instance;
        animator = GetComponent<Animator>();
    }

	void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Yam")
        {
            Destroy(other.gameObject);
            if (rendera.sprite != munchFrame)
            {
                rendera.sprite = munchFrame;
                animator.enabled = false;
                Invoke("Unmunch", munchTime);
            }
        }
        else if (other.tag == "BadYam")
        {
            rendera.sprite = deadFrame;
            HandleAnimalDeath();
            animator.enabled = false;
        }
        else if (other.tag == "Saw")
        {
            rendera.sprite = deadFrame;
            HandleAnimalDeath();
            animator.enabled = false;
        }
    }

    public void Anger()
    {
        rendera.sprite = angryFrame;
        animator.enabled = false;
        Invoke("Unmunch", angryTime);
    }

    void HandleAnimalDeath()
    {
        GameObject[] yams = GameObject.FindGameObjectsWithTag("Yam");
        GameObject[] badYams = GameObject.FindGameObjectsWithTag("BadYam");
        GameObject[] spawners = GameObject.FindGameObjectsWithTag("Spawner");
        GameObject.Find("Lives").GetComponent<IconStrip>().Mark();
        manager.GetComponent<ScreenShake>().ShakeScreen(deathScreenShakeDuration);

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

        manager.IncrementLostAnimals();
    }

    void Unmunch()
    {
        rendera.sprite = waitFrame;
        animator.enabled = true;
    }
}
