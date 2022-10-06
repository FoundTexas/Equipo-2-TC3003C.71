using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlanetCrashUI
{
    public class MiniMap : MonoBehaviour
    {
        [Tooltip("Reference to the Player's GameObject")]
        public GameObject player;

        void Awake()
        {
            player = GameObject.FindWithTag("Player");
        }
        //Uses LateUpdate to update the minimap after the player moves
        void LateUpdate()
        {
            Vector3 newPos = player.transform.position;
            newPos.y = transform.position.y;
            transform.position = newPos;
        }
    }
}
