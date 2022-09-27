using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

/// <summary>
/// Class that contains the hitStop effect behaviour.
/// </summary>
public class HitStop : MonoBehaviour
{
    [Min(0)] public float hitStopDuration = 1f;
    [Min(0)] public float remainingHitStop = 0f;
    public CinemachineCameraOffset cinemachineCamera;
    public float shakeMagnitude;
    public float shakeDuration;
    private bool inHitStop = false;

    // ----------------------------------------------------------------------------------------------- Unity Methods
    void Update()
    {
        if(remainingHitStop > 0 && !inHitStop)
            StartCoroutine(DoHitStop());
    }

    // ----------------------------------------------------------------------------------------------- Public Methods
    /// <summary>
    /// Method that determines the shake values and the hitStop time.
    /// </summary>
    /// <param name="magnitude"> float value that determines how strong the shake will be. </param>
    /// <param name="duration"> float value that determines the time of the hitStop. </param>
    public void HitStopFreeze(float magnitude, float duration)
    {
        shakeMagnitude = magnitude;
        shakeDuration = duration;
        remainingHitStop = hitStopDuration;
    }

    // ----------------------------------------------------------------------------------------------- Private Coroutines
    /// <summary>
    /// This coroutine creates the hitStop effect depending on the shake and duration values.
    /// </summary>
    private IEnumerator DoHitStop()
    {
        inHitStop = true;
        float original = Time.timeScale;
        Time.timeScale = 0f;
        StartCoroutine(ScreenShake());
        yield return new WaitForSecondsRealtime(hitStopDuration);

        Time.timeScale = original;
        remainingHitStop = 0;
        inHitStop = false;
    }

    /// <summary>
    /// This coroutine creates screen shake effect 
    /// to the cinemachine camera with a specific magnitude.
    /// </summary>
    private IEnumerator ScreenShake()
    {
        Vector3 originalPos = cinemachineCamera.m_Offset;
        float timeShaking = 0.0f;
        while(timeShaking < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;
            cinemachineCamera.m_Offset = new Vector3(x, y, originalPos.z);

            timeShaking += Time.deltaTime;
            yield return null;
        }
        cinemachineCamera.m_Offset = originalPos;
    }
}
