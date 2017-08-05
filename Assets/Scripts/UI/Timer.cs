using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour {
    public float urgencyScale;
    public int urgencyThreshold;

    int displayedTime;
    float remainingTime;
    RectTransform rt;
    Text text;

    private void Awake ()
    {
        rt = GetComponent<RectTransform>();
        text = GetComponent<Text>();
        displayedTime = int.Parse(text.text);
    }

    public void UpdateTime(float remainingTime)
    {
        this.remainingTime = remainingTime;
        int remainingIntTime = (int)Mathf.Floor(this.remainingTime);
        float remainingFract = remainingTime - remainingIntTime;
        remainingIntTime++;

        if (remainingIntTime != displayedTime)
        {
            displayedTime = remainingIntTime;
            text.text = remainingIntTime.ToString();
        }
        if (remainingIntTime <= urgencyThreshold)
        {
            float scale = Mathf.Lerp(1, urgencyScale, remainingFract);
            rt.localScale = new Vector2(scale, scale);
        }
    }
}
