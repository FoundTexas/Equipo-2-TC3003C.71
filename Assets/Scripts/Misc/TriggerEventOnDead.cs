using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventOnDead : MonoBehaviour
{
    public UnityEvent TriggerEvent;
    bool isApplicationQuitting = false;

    public void SetTrigger()
    {
        TriggerEvent.Invoke();
    }

    private void OnDestroy()
    {
        if (isApplicationQuitting)
        {
            return;
        }
        else if (!isApplicationQuitting)
        {
            SetTrigger();
        }
    }

    void OnApplicationQuit()
    {
        isApplicationQuitting = true;
    }
}
