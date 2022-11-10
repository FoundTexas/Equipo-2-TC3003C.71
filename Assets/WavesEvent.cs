using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class WavesEvent : UnityEvent<Waves>
{
    UnityEvent wavesEvent;

    void Start()
    {
        if (wavesEvent == null)
        {
            wavesEvent = new UnityEvent();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && wavesEvent != null)
        {
            wavesEvent.Invoke();
        }
    }


}
