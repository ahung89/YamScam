using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Icon : MonoBehaviour {

    public float rotationAmount;
    public float scaleAmount;
    public float wiggleDuration;
    public float rotationFrequencyMultiplier;

    bool marked;
    bool wiggling = false;
    float wiggleStartTime;
    RectTransform rt;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

	public bool Mark()
    {
        if (marked)
            return false;

        marked = true;
        GetComponent<Image>().color = Color.red;
        wiggling = true;
        wiggleStartTime = Time.time;
        return true;
    }

    public bool Marked()
    {
        return marked;
    }

    void Update()
    {
        if (wiggling)
        {
            float dt = Time.time - wiggleStartTime;
            rt.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotationAmount * Mathf.Sin(rotationFrequencyMultiplier * (Time.time - wiggleStartTime))));
            float scaleAddend = scaleAmount * Mathf.Sin(dt * ((Mathf.PI / 1) / wiggleDuration));
            float scale = 1 + scaleAddend;
            rt.transform.localScale = new Vector3(scale, scale, 1);

            if (dt > wiggleDuration)
            {
                wiggling = false;
                rt.transform.rotation = Quaternion.identity;
            }
        }
    }

    void Wiggle()
    {

    }
}
