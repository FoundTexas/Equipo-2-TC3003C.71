using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public CheckPoint[] others;
    public Animation anim;
    public Vector3 pos;
    public bool collected = false, saver = false;
    public Color selected, deselected;
    public Renderer render;
    private void Start()
    {
        others = FindObjectsOfType<CheckPoint>();
        pos = transform.position + transform.up;
    }

    private void Update()
    {
        if (collected && !anim.isPlaying)
        {
            anim.Play();
            render.material.color = selected;
        }
        else if (!collected && anim.isPlaying)
        {
            anim.Stop();
            render.material.color = deselected;
        }

    }
    void disableval()
    {
        collected = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.setCheckPoint(pos);
            collected = true;

            foreach(var cp in others)
            {
                if (cp.gameObject != this.gameObject)
                {
                    cp.disableval();
                }
            }

            if (saver)
            {
                GameManager.SaveGame();
            }
        }
    }
}
