using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
[Serializable]
[CreateAssetMenu(fileName = "d_Interaction", menuName = "ScriptableObjects/Dialogue", order = 1)]

public class DialogueInteraction : ScriptableObject 
{
    [SerializeField] List<Line> lines = new List<Line>();
    [SerializeField] UnityEvent startEvent, endEvent;
    [SerializeField] string buttonsText;
    [SerializeField] List<DialogueInteraction> Interactions = new List<DialogueInteraction>();


    public void StartInteraction()
    {
        startEvent.Invoke();
    }

    public bool lineContinue(int index)
    {
        return index < lines.Count;
    }

    public Line getLine(int index)
    {
        return lines[index];
    }

    public List<DialogueInteraction> getInteractions()
    {
        return Interactions;
    }

    public void EndInteraction()
    {
        endEvent.Invoke();
    }

}

