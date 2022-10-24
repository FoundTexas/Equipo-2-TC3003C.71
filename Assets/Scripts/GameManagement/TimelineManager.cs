using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    PlayableDirector director;
    Move player;
    bool played;

    void Start()
    {
        played = false;
        director = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (director.time > 0 && !played)
        {
            played = true;
            player = GameManager.GetLocalPlayer().GetComponent<Move>();
            if (player)
            {
                player.canMove = false;
                player.StopMove();
            }
        }
        else if (director.time <= 0 && played)
        {
            played = false;
            player = GameManager.GetLocalPlayer().GetComponent<Move>();
            if (player)
            {
                player.canMove = true;
            }
        }
    }
}
