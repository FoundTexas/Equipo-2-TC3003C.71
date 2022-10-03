using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]

public class Line 
{
   
    [SerializeField] string speakerName;
    [SerializeField] string dialogue;
    [SerializeField] AudioClip soundEffect;
    /// <summary>
    /// This function return the dialogues of the interaction
    /// </summary>
    /// <returns></returns>
    public string getLine()
    {
        return dialogue;
    }
    /// <summary>
    /// This function return the name of the NPC that is having the interaction
    /// </summary>
    /// <returns></returns>
    public string getName()
    {
        return speakerName;
    }

    /// <summary>
    /// This function return the Audio clip of the interactions
    /// </summary>
    /// <returns></returns>
    public AudioClip getSound()
    {
        return soundEffect;
    }
    
}
