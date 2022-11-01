using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    PlayableDirector director;
    Move player;
    bool played;

    public static bool enemiesCanMove;

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
            enemiesCanMove = false;
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
            enemiesCanMove = true;
            played = false;
            player = GameManager.GetLocalPlayer().GetComponent<Move>();
            if (player)
            {
                player.canMove = true;
            }
        }
    }
}
