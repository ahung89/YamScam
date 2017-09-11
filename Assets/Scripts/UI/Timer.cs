using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    public float urgencyScale;
    public int urgencyThreshold;
    public int gameStartUrgencyThreshold = 5;
    public List<AudioClip> beeps;

    private int displayedTime;
    private float remainingTime;
    private RectTransform rt;
    private Text text;
    private AudioSource audioSource;
    private float initialScale;

    GameManager gameManager;

    private void Awake ()
    {
        rt = GetComponent<RectTransform>();
        text = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
        displayedTime = int.Parse(text.text);
        gameManager = GameManager.Instance;
        initialScale = text.transform.localScale.x;
        urgencyScale *= initialScale;

        audioSource.PlayOneShot(beeps[0]);
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
