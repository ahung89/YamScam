using System;
using UnityEngine;

public static class PlayerDataManager
{
    private const string LevelProgressKey = "LevelProgressKey";
    private const int DefaultLevelProgress = 1;

    private const string YamScoreKey = "YamScoreKey";
    private const int DefaultYamScore = 0;

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

    public static int YamScore 
    {
        get 
        { 
            return PlayerPrefs.GetInt(YamScoreKey, DefaultYamScore); 
        }
        set 
        { 
            PlayerPrefs.SetInt(YamScoreKey, value);
        }
    }
}

