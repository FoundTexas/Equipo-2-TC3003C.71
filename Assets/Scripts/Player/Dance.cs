using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dance : MonoBehaviour
{
    Animator anim;
    public Move playerMove;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Dance"))
            playerMove.enabled = false;
            
        else
            playerMove.enabled = true;
    }
}
