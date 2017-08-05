using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ScoreManager : MonoBehaviour {

    public float remainingTime;
    public int goodYamLossLimit;

    int numAnimalsLost = 0;
    int totalAnimalCount = 0;
    int yamsLost = 0;

    void Awake()
    {
        numAnimalsLost = GameObject.FindGameObjectsWithTag("Animal").Length;
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;
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
