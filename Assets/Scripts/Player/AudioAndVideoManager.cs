using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioAndVideoManager : MonoBehaviour
{
    [SerializeField] float IdelTime;
    [SerializeField] AudioClip Land, jump, unlock;
    [SerializeField] AudioClip[] step, step2;
    [SerializeField] string[] grounds;
    Dictionary<string, int> ground = new Dictionary<string, int>();
    AudioSource audios;
    [SerializeField] Animator anim;
    float curgunval = 0, gunset = -1;

    private void Start()
    {
        for(int i = 0; i < grounds.Length; i++)
        {
            ground.Add(grounds[i], i);
        }
        audios = GetComponent<AudioSource>();
    }
    private void Update()
    {
        ChangeGun();
    }
    /// <summary>
    /// Plays sound of a step when called from the player Animator.
    /// </summary>
    public void StepSound(string floor)
    {
        int tmp = ground[floor];
        audios.PlayOneShot(
            Random.Range(0,2) == 0? step[tmp] : step2[tmp]
            );
    }
    /// <summary>
    /// Plays unlock sound when inputting the secret code when called from the player Animator.
    /// </summary>
    public void UnlockSound()
    {
        audios.PlayOneShot(unlock);
    }
    /// <summary>
    /// Plays Robot�s jump sound when called.
    /// </summary>
    public void jumpSound()
    {
        audios.PlayOneShot(jump);
        anim.SetTrigger("Jump");
    }
    public void DiveSound()
    {
        audios.PlayOneShot(jump);
        anim.SetTrigger("Jump");
        anim.SetTrigger("Jump2");
    }
    /// <summary>
    /// Function that sets the Animator "isGround" boolean and sets landing sound if is the case.
    /// </summary>
    /// <param name="grounded">State sent and compared with Animator state. </param>
    public void IsOnGround(bool grounded)
    {
        if (!anim.GetBool("isGrounded") && grounded)
        {
            audios.PlayOneShot(Land);
        }
        anim.SetBool("isGrounded", grounded);
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
    /// Function that sets the Animator "hasGun" boolean and sets The gun�s sound if is the case.
    /// </summary>
    /// <param name="gun"> State sent and compared with Animator state. </param>
    /// <param name="sound"> AudioClip from the Weapon. </param>
    public void GunValue(bool gun, AudioClip sound)
    {
        if (anim.GetFloat("hasGun") == 0 && gun)
        {
            audios.PlayOneShot(sound);
        }
        anim.SetFloat("hasGun", gun? 1 : 0);
    }
    void ChangeGun()
    {
        if (gunset > 0)
        {
            if (curgunval > 0)
            {

            }
        }
        else if (gunset < 0)
        {
            if (curgunval > 0)
            {

            }
        }
    }
    /// <summary>
    /// Function that sets Animator "speed" Float.
    /// </summary>
    /// <param name="speed"> State sent and compared with Animator state. </param>
    public void SetIfMovement(float speed)
    {
        anim.SetFloat("speed", speed);
        anim.SetFloat("IdelTime", IdelTime);

        if (speed <= 0.3f)
        {
            if (IdelTime > 40)
            {
                IdelTime = 0;
            }
            else
            {
                IdelTime += Time.deltaTime;
            }
        }
        else
        {
            IdelTime = 0;
        }
    }
}
