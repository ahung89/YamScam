using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public float remainingTime;
    public int goodYamLossLimit = 3;

    int numAnimals;
    int numAnimalsLost = 0;
    int totalAnimalCount = 0;
    int yamsLost = 0;

    Timer timer;

    void Awake()
    {
        numAnimals = GameObject.FindGameObjectsWithTag("Animal").Length;
        timer = GameObject.Find("Timer").GetComponent<Timer>();
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;
        timer.UpdateTime(remainingTime);
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
