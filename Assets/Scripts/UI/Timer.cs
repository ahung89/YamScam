using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    public float urgencyScale;
    public int urgencyThreshold;
    public int gameStartUrgencyThreshold = 5;

    int displayedTime;
    float remainingTime;
    RectTransform rt;
    Text text;

    GameManager gameManager;

    private void Awake ()
    {
        rt = GetComponent<RectTransform>();
        text = GetComponent<Text>();
        displayedTime = int.Parse(text.text);
        gameManager = GameManager.Instance;
    }

    public void UpdateTime (float remainingTime, bool gameStartCountdown = false)
    {
        this.remainingTime = remainingTime;
        int remainingIntTime = (int)Mathf.Floor(this.remainingTime);
        float remainingFract = remainingTime - remainingIntTime;
        int threshold = gameStartCountdown ? gameStartUrgencyThreshold : urgencyThreshold;
        remainingIntTime += gameStartCountdown ? 0 : 1;

        if (remainingIntTime != displayedTime)
        {
            displayedTime = remainingIntTime;
            if (gameStartCountdown)
            {
                int beepIndex = 3 - remainingIntTime;
                if (beepIndex >= 0 && beepIndex <= 2)
                {
                    gameManager.beeps[beepIndex].Play();
                }
                else if (remainingIntTime == 0)
                {
                    gameManager.song.Play();
                }
                text.text = remainingIntTime == 0 ? "GO" : remainingIntTime.ToString();
            }
            else if (remainingIntTime != 0)
            {
                text.text = remainingIntTime.ToString();
            }
            else
            {
                text.text = "";
            }
        }
        if (remainingIntTime <= threshold)
        {
            float scale = Mathf.Lerp(1, urgencyScale, remainingFract);
            rt.localScale = new Vector2(scale, scale);
        }
        else
        {
            rt.localScale = new Vector2(1, 1);
        }
    }
}
