﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class JacketMenu : MonoBehaviour {

    public GameObject worldMapMenu;

    public void TransitionToWorldMap()
    {
        MenuManager.Instance.PushPanel(worldMapMenu);
    }

    public void StartGame ()
    {
		SceneManager.LoadSceneAsync(PlayerDataManager.LevelProgress);
    }

    public void ExitGame ()
    {
        Application.Quit();
    }
}
