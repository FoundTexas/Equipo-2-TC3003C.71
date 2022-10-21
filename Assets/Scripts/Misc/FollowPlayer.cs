using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] public Transform Player;
    void Update()
    {
        transform.position = Player.position;
    }
}
