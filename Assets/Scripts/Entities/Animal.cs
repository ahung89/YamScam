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
    ScreenShake screenShake;

    void Awake()
    {
        rendera = GetComponent<SpriteRenderer>();
        manager = GameManager.Instance;
        animator = GetComponent<Animator>();
        screenShake = manager.GetComponent<ScreenShake>();
    }

	void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == TagNames.YAM)
        {
            Destroy(other.gameObject);
            if (rendera.sprite != munchFrame)
            {
                rendera.sprite = munchFrame;
                animator.enabled = false;
                Invoke("Unmunch", munchTime);
            }
        }
        else if (other.tag == TagNames.BAD_YAM)
        {
            rendera.sprite = deadFrame;
            HandleAnimalDeath();
            animator.enabled = false;
        }
        else if (other.tag == TagNames.SAW)
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
        EventBus.PublishEvent(new BeastKilledEvent(gameObject));
        screenShake.ShakeScreen(deathScreenShakeDuration);
    }

    void Unmunch()
    {
        rendera.sprite = waitFrame;
        animator.enabled = true;
    }
}
