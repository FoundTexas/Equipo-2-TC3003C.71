using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] public Transform Player;
    void Update()
    {
        if(Player != null)
            transform.position = Player.position;
    }
    public void setFollow(Transform target)
    {
        Player = target;
    }
}
