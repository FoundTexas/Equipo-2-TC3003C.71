using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelineAnimController : MonoBehaviour
{
    Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    public void play(AnimationClip a)
    {
        anim.Play(a.name);
    }
}
