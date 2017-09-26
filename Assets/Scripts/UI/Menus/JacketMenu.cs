using System.Collections;
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
        SceneManager.LoadSceneAsync(0);
    }

    public void ExitGame ()
    {
        Application.Quit();
    }
}
