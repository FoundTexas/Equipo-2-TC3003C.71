using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class NPC : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Rig turnRig;
    [SerializeField] float MaxDist, distance  = Mathf.Infinity;

    bool Locked;

    private void Update()
    {
        GetTarget();
    }

    void GetTarget()
    {
        turnRig.weight = Mathf.Clamp(MaxDist-distance, 0.0f, 1.0f);
        if (Locked)
            return;

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        distance  = Mathf.Infinity;
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
