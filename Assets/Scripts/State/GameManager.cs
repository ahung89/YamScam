using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float remainingTime;
    public float gameBeginCountdownTime = 3;
    public int goodYamLossLimit = 3;

    int numAnimals;
    int numAnimalsLost = 0;
    int totalAnimalCount = 0;
    int yamsLost = 0;

    Timer timer;
    bool gameStarted = false;

    void Awake()
    {
        EventBus.Reset();
        numAnimals = GameObject.FindGameObjectsWithTag("Animal").Length;
        timer = GameObject.Find("Timer").GetComponent<Timer>();
    }

    void Update()
    {
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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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