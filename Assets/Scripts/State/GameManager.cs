using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public float remainingTime;
    public float gameBeginCountdownTime = 3;
    public int goodYamLossLimit = 3;
    public int difficulty;
    public float fadeAlphaPerSec;

    int numAnimals;
    int numAnimalsLost = 0;
    int totalAnimalCount = 0;
    int yamsLost = 0;

    Timer timer;
    bool gameStarted = false;
    bool fadeInFinished = false;
    bool gameEnded = false;
    float fadeStartTime;
    Image fadePanel;

    void Awake()
    {
        Debug.Log("scene loaded");
        EventBus.Reset();
        numAnimals = GameObject.FindGameObjectsWithTag("Animal").Length;
        timer = GameObject.Find("Timer").GetComponent<Timer>();
        fadePanel = GameObject.Find("FadePanel").GetComponent<Image>();
        fadeStartTime = Time.time;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn ()
    {
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, Mathf.Lerp(1, 0, (Time.time - fadeStartTime) * fadeAlphaPerSec));
        yield return null;
        if (fadePanel.color.a > 0)
        {
            StartCoroutine(FadeIn());
        }
        else
        {
            fadeInFinished = true;
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    void Update()
    {
        if (gameEnded)
            return;

        if (gameBeginCountdownTime > 0)
        {
            gameBeginCountdownTime -= Time.deltaTime;
            if (gameBeginCountdownTime >= 0 && Time.deltaTime != 0)
            {
                timer.UpdateTime(gameBeginCountdownTime, true);
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

        if (remainingTime <= 0)
        {
            gameEnded = true;
            fadeStartTime = Time.time;
            StartCoroutine(FadeOut());
        }
    }

    public void IncrementGoodYamLost()
    {
        yamsLost++;
        if (yamsLost == goodYamLossLimit)
        {
            Debug.Log("YOU LOST YO");
        }
    }

    public void IncrementLostAnimals()
    {
        numAnimalsLost++;
    }
}

public struct GameStartedEvent { }