using System;
using UnityEngine;

public static class PlayerDataManager
{
	private const string LevelProgressKey = "LevelProgressKey";
	private const int DefaultLevelProgress = 0;

	public static int LevelProgress 
	{
		get 
		{ 
			return PlayerPrefs.GetInt(LevelProgressKey, DefaultLevelProgress); 
		}
		set 
		{ 
			PlayerPrefs.SetInt(LevelProgressKey, value); 
		}
	}
}

