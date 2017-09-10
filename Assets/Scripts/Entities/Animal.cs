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
    public AudioClip angryOink;

    SpriteRenderer rendera;
    GameManager manager;
    Animator animator;
    ScreenShake screenShake;
    AudioSource audioSource;

    void Awake()
    {
        rendera = GetComponent<SpriteRenderer>();
        manager = GameManager.Instance;
        animator = GetComponent<Animator>();
        screenShake = manager.GetComponent<ScreenShake>();
        audioSource = GetComponent<AudioSource>();
    }

	void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == TagNames.YAM)
        {
            if (other.GetComponent<Yam>().isBad)
            {
                rendera.sprite = deadFrame;
                HandleAnimalDeath();
                animator.enabled = false;
            }
            else
            {
                Destroy(other.gameObject);
                if (rendera.sprite != munchFrame)
                {
                    rendera.sprite = munchFrame;
                    animator.enabled = false;
                    Invoke("Unmunch", munchTime);
                }
            }
        }
        else if (other.tag == TagNames.SAW)
        {
            rendera.sprite = deadFrame;
            HandleAnimalDeath();
            animator.enabled = false;
        }
    }

    [SubscribeGlobal]
    public void HandleGoodYamLost(GoodYamLostEvent e)
    {
        Anger();
    }

    public void Anger()
    {
        rendera.sprite = angryFrame;
        animator.enabled = false;
        Invoke("Unmunch", angryTime);
        audioSource.PlayOneShot(angryOink);
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
