using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class JacketMenu : MonoBehaviour {

    public ZoomablePanel worldMapMenu;

    public void TransitionToWorldMap()
    {
        MenuManager.Instance.ZoomIntoPanel(worldMapMenu);

		// TODO: callback to load WorldMapScene after jacket map is fully zoomed
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
