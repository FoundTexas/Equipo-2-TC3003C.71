using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class AudioAndVideoManager : MonoBehaviour
{
    [SerializeField] float IdelTime;
    [SerializeField] AudioClip Land, jump, step;
    AudioSource audios;
    Animator anim;

    private void Start()
    {
        audios = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }
    /// <summary>
    /// Plays sound of a step when called from the player Animator.
    /// </summary>
    public void StepSound()
    {
        audios.PlayOneShot(step);
    }
    /// <summary>
    /// Plays Robot´s jump sound when called.
    /// </summary>
    public void jumpSound()
    {
        audios.PlayOneShot(jump);
    }
    /// <summary>
    /// Function that sets the Animator "isGround" boolean and sets landing sound if is the case.
    /// </summary>
    /// <param name="grounded">State sent and compared with Animator state. </param>
    public void IsOnGround(bool grounded)
    {
        if (!anim.GetBool("isGround") && grounded)
        {
            audios.PlayOneShot(Land);
        }
        anim.SetBool("isGround",grounded);
    }
    /// <summary>
    /// Function that sets the Animator "onWall" boolean and sets landing sound if is the case.
    /// </summary>
    /// <param name="wall"> State sent and compared with Animator state. </param>
    public void IsOnWall(bool wall)
    {
        if (!anim.GetBool("onWall") && wall)
        {
            audios.PlayOneShot(Land);
        }
        anim.SetBool("onWall", wall);
    }
    /// <summary>
    /// Function that sets the Animator "hasGun" boolean and sets The gun´s sound if is the case.
    /// </summary>
    /// <param name="gun"> State sent and compared with Animator state. </param>
    /// <param name="sound"> AudioClip from the Weapon. </param>
    public void GunValue(bool gun, AudioClip sound)
    {
        if (!anim.GetBool("hasGun") && gun)
        {
            audios.PlayOneShot(sound);
        }
        anim.SetBool("hasGun", gun);
    }
    /// <summary>
    /// Function that sets Animator "speed" Float.
    /// </summary>
    /// <param name="speed"> State sent and compared with Animator state. </param>
    public void SetIfMovement(float speed)
    {
        anim.SetFloat("speed", speed);

        if(speed <= 0)
        {
            anim.SetFloat("IdelTime", IdelTime);
            if (IdelTime > 40)
            {
                IdelTime = 0;
            }
            else
            {
                IdelTime += Time.deltaTime;
            }
        }
    }
}
