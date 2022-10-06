using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    public float amplitudeGain; 
    public float frequemcyGain; 
    public CinemachineFreeLook cmFreeCam; 
    public float shakeDuration;

    private void Awake()
    {
        Instance = this;
        cmFreeCam = GetComponent<CinemachineFreeLook>();
    }

    public void DoShake(float amplitudeGain, float frequemcyGain, float shakeDuration)
    {
        this.amplitudeGain = amplitudeGain;
        this.frequemcyGain = frequemcyGain;
        this.shakeDuration = shakeDuration;

        StartCoroutine(Shake());
    }
    public IEnumerator Shake()
    {
        Noise(amplitudeGain, frequemcyGain);
        yield return new WaitForSeconds(shakeDuration);
        Noise(0, 0);
    }
    void Noise(float amplitude, float frequency)
    {
        cmFreeCam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        cmFreeCam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        cmFreeCam.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = amplitude;
        cmFreeCam.GetRig(0).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
        cmFreeCam.GetRig(1).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
        cmFreeCam.GetRig(2).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = frequency;
    }
}
