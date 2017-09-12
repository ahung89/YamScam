using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public float remainingTime;
    public int difficulty;
    public float difficultyMultiplier = 1;
    public float fadeAlphaPerSec;
    public AudioClip winSound;
    public AudioClip loseSound;
    public Timer timer;
    public Image fadePanel;
    public bool gameOver = false;
    public AudioSource musicSource;
    public GameObject gameOverPanel;

    private int numAnimalsLost = 0;
    private static int numAnimalLives = 3;
    private static int goodYamLossLimit = 3;
    private int yamsLost = 0;
    private bool gameStarted = false;
    private bool gameEnded = false;
    private float fadeStartTime;
    private AudioSource winLoseEffectsSource;
    private float remainingCountdownTime;

    void Awake ()
    {
        remainingCountdownTime = timer.gameBeginCountdownTime;

        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            return;
        }

        Instance = this;

        Screen.SetResolution(450, 800, false);
        EventBus.Reset();
        fadeStartTime = Time.time;
        StartCoroutine(FadeIn());

        winLoseEffectsSource = GetComponent<AudioSource>();
    }

    IEnumerator FadeIn ()
    {
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, Mathf.Lerp(1, 0, (Time.time - fadeStartTime) * fadeAlphaPerSec));
        yield return null;
        if (fadePanel.color.a > 0)
        {
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeOut ()
    {
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, Mathf.Lerp(0, 1, (Time.time - fadeStartTime) * fadeAlphaPerSec));
        yield return null;
        if (fadePanel.color.a < 1)
        {
            StartCoroutine(FadeOut());
        }
        else
        {
            if (gameOver)
            {
                gameOverPanel.GetComponent<Canvas>().enabled = true;
                musicSource.Stop();
                Time.timeScale = 0;
                winLoseEffectsSource.PlayOneShot(loseSound);
            }
            else
            {
                yield return new WaitForSeconds(2);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    void Update()
    {
        if (gameEnded)
            return;

        if (remainingCountdownTime > 0)
        {
            remainingCountdownTime -= Time.deltaTime;
            if (remainingCountdownTime >= 0 && Time.deltaTime != 0)
            {
                timer.UpdateTime(remainingCountdownTime, true);
            }
        }
        else
        {
            if (!gameStarted)
            {
                EventBus.PublishEvent(new GameStartedEvent());
                gameStarted = true;
            }
            remainingTime -= Time.deltaTime;
            timer.UpdateTime(remainingTime);
        }

        if (remainingTime <= 0 && !gameOver)
        {
            gameEnded = true;
            fadeStartTime = Time.time;
            StartCoroutine(FadeOut());

            musicSource.Stop();
            winLoseEffectsSource.PlayOneShot(winSound);
        }
    }

    [SubscribeGlobal]
    public void HandleGoodYamLost(GoodYamLostEvent e)
    {
        yamsLost++;
        if (yamsLost >= goodYamLossLimit && !gameOver)
        {
            gameOver = true;
            gameEnded = true;
            fadeStartTime = Time.time;
            StartCoroutine(FadeOut());
        }
    }

    [SubscribeGlobal]
    public void HandleBeastKilledEvent(BeastKilledEvent e)
    {
        IncrementLostAnimals();
    }

    public void PlaySong()
    {
        musicSource.Play();
    }

    void IncrementLostAnimals()
    {
        numAnimalsLost++;
        if (numAnimalsLost == numAnimalLives && !gameOver)
        {
            if (SceneManager.GetActiveScene().buildIndex == 6)
            {
                gameOver = true;
                gameEnded = true;
                fadeStartTime = Time.time;
                StartCoroutine(FadeOut());
            }
        }
    }
}

public struct GameStartedEvent { }