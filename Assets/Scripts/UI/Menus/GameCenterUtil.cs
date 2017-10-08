using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenterUtil : MonoBehaviour
{
    public static string YamsGameLeaderBoardID = "YamsGameLeaderboard";

    public void AuthenticateToGameCenter()
    {
        #if UNITY_IOS

        Social.localUser.Authenticate(authenticationSucceeded => {
            Debug.Log(authenticationSucceeded ? "User Authentication succeeded." : "User Authentication failed.");
        });

        #endif
    }

    public static void ReportScore(long score, string leaderboardID)
    {
        #if UNITY_IOS

        if (Social.localUser.authenticated)
        {
            Social.ReportScore(score, leaderboardID, success => {                   
                Debug.Log(success ? "Score report succeeded." : "Failed to report score.");
            });
        }
        else
        {
            Social.localUser.Authenticate(authenticationSucceeded => {
                if (authenticationSucceeded)              
                {
                    Social.ReportScore(score, leaderboardID, success => {                   
                        Debug.Log(success ? "Score report succeeded." : "Failed to report score.");
                    });
                }
                else
                {
                    Debug.Log("User Authentication failed.");
                }
            });
        }

        #endif
    }

    public void ShowLeaderboard()
    {
        #if UNITY_IOS

        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            Social.localUser.Authenticate(authenticationSucceeded => {
                    
                if (authenticationSucceeded)              
                {
                    Social.ShowLeaderboardUI();
                }
                else
                {
                    Debug.Log("User Authentication failed.");
                }
            });
        }

        #endif
    }
}
