using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour {

    public float screenShakeAmount = .1f;
    public float timeBetweenShakes = .1f;
    public float shakeDuration;

    Vector3 originalCameraPos;
    float shakeStartTime;

    void Awake()
    {
        originalCameraPos = Camera.main.transform.position;
    }

	public void ShakeScreen(float shakeDuration)
    {
        this.shakeDuration = shakeDuration;
        shakeStartTime = Time.time;
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        Vector3 newCameraPos = originalCameraPos + (Vector3)(Random.insideUnitCircle * screenShakeAmount);
        newCameraPos.z = originalCameraPos.z;
        Camera.main.transform.position = newCameraPos;
        yield return new WaitForSeconds(timeBetweenShakes);

        if (Time.time - shakeStartTime < shakeDuration)
        {
            StartCoroutine(ShakeCoroutine());
        }
        else
        {
            Camera.main.transform.position = originalCameraPos;
        }
    }
}
