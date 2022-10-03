using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Animations.Rigging;

public class NPC : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Rig turnRig;
    [SerializeField] float MaxDist, distance = Mathf.Infinity;
    public List<DialogueInteraction> interactions;
    public int index;
    public bool Locked;
    DialogueManager manager;

    private void Start()
    {
        manager = FindObjectOfType<DialogueManager>();
    }
    private void Update()
    {
        GetTarget();
        InteractionHandler();
    }

    void InteractionHandler()
    {
        if (DialogueManager.Ended)
        {
            if (Input.GetKeyDown(KeyCode.E) && !Locked)
            {
                if (interactions.Count >= index)
                {
                    if (distance <= MaxDist)
                    {
                        Locked = true;
                        manager.StartInteraction(interactions[index]);
                    }
                }
            }
        }
    }


    void GetTarget()
    {
        turnRig.weight = Mathf.Clamp(MaxDist - distance, 0.0f, 1.0f);
        if (Locked)
            return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        distance = Mathf.Infinity;
        foreach (GameObject player in players)
        {
            float newDistance = Vector3.Distance(transform.position, player.transform.position);
            if (newDistance <= distance)
            {
                distance = Mathf.Abs(newDistance);
                target.position = player.transform.position;
            }
        }
    }
}
