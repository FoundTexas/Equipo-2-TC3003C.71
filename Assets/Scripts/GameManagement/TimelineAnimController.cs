using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TimelineAnimController : MonoBehaviour
{
    Animator anim;

    private void Start() {
        anim = GetComponent<Animator>();
    }

    public void play(AnimationClip a)
    {
        // if (GameManager.isOnline)
        // {
        //     PhotonView PV = GetComponent<PhotonView>();
        //     PV.RPC("PunRPCSetting", RpcTarget.All, a.name);
        // }
        // else if (!GameManager.isOnline)
        // {
        //     PunRPCPlay(a.name);
        // }
        anim.Play(a.name);
    }

    [PunRPC]
    public void PunRPCPlay(string a)
    {
        anim.Play(a);
    }
}
