using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WorldMapManager : MonoBehaviour {

	public void LoadLevel(int levelId)
	{
		// load level if it is unlocked
		if (levelId <= PlayerDataManager.LevelProgress) 
		{
			SceneManager.LoadSceneAsync(levelId);
		}
	}
}
