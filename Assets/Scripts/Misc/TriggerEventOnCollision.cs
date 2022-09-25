using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventOnCollision : MonoBehaviour
{
    public UnityEvent TriggerEvent;

    public void SetTrigger()
    {
        TriggerEvent.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetTrigger();
        }
    }
}
