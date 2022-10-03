using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that adds the look at camera effect.
/// </summary>
public class LookAtCamera : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------- Unity Methods
    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
