using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class HitStop : MonoBehaviour
{

    public float hitStopDuration = 1f;
    public float remainingHitStop = 0f;
    bool inHitStop = false;
    public CinemachineCameraOffset cinemachineCamera;
    public float shakeMagnitude;
    public float shakeDuration;

    void Update()
    {
        if(remainingHitStop > 0 && !inHitStop)

            StartCoroutine(DoHitStop());
    }

    public void HitStopFreeze()
    {
        remainingHitStop = hitStopDuration;
    }

    public IEnumerator DoHitStop()
    {
        inHitStop = true;
        var original = Time.timeScale;
        Time.timeScale = 0f;
        StartCoroutine(ScreenShake(shakeMagnitude, shakeDuration));
        yield return new WaitForSecondsRealtime(hitStopDuration);

        Time.timeScale = original;
        remainingHitStop = 0;
        inHitStop = false;
    }

    public IEnumerator ScreenShake(float magnitude, float duration)
    {
        Vector3 originalPos = cinemachineCamera.m_Offset;
        Debug.Log("Camera X offset is: " + originalPos.x);
        Debug.Log("Camera Y offset is: " + originalPos.y);
        Debug.Log("Camera Z offset is: " + originalPos.z);
        float timeShaking = 0.0f;
        while(timeShaking < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            cinemachineCamera.m_Offset = new Vector3(x, y, originalPos.z);

            timeShaking += Time.deltaTime;
            yield return null;
        }
        
        cinemachineCamera.m_Offset = originalPos;
    }
}
