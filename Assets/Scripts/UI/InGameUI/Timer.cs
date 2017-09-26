using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    public float urgencyScale;
    public int urgencyThreshold;
    public int gameStartUrgencyThreshold;
    public List<AudioClip> beeps;
    public float gameBeginCountdownTime;

    private int displayedTime;
    private float remainingTime;
    private RectTransform rt;
    private Text text;
    private AudioSource audioSource;
    private float initialScale;
    private GameManager gameManager;
    private bool firstBeep;

    private void Awake ()
    {
        firstBeep = false;
        rt = GetComponent<RectTransform>();
        text = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
        displayedTime = int.Parse(text.text);
        gameManager = GameManager.Instance;
        initialScale = text.transform.localScale.x;
        urgencyScale *= initialScale;

    }

    public void UpdateTime (float remainingTime, bool gameStartCountdown = false)
    {
        this.remainingTime = remainingTime;

        if (!firstBeep)
        {
            audioSource.PlayOneShot(beeps[0]);
            firstBeep = true;
        }

        int remainingIntTime = (int)Mathf.Floor(this.remainingTime);
        float remainingFract = remainingTime - remainingIntTime;
        int threshold = gameStartCountdown ? gameStartUrgencyThreshold : urgencyThreshold;
        remainingIntTime += gameStartCountdown ? 0 : 1;

        if (remainingIntTime != displayedTime)
        {
            displayedTime = remainingIntTime;
            if (gameStartCountdown)
            {
                int beepIndex = beeps.Count - remainingIntTime;
                if (beepIndex >= 0 && beepIndex <= beeps.Count - 1)
                {
                    audioSource.PlayOneShot(beeps[beepIndex]);
                }
                else if (remainingIntTime == 0)
                {
                    gameManager.PlaySong();
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
            float scale = Mathf.Lerp(initialScale, urgencyScale, remainingFract);
            rt.localScale = new Vector2(scale, scale);
        }
        else
        {
            rt.localScale = new Vector2(initialScale, initialScale);
        }
    }
}
