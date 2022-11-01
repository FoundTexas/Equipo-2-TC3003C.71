using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    public Animator leftDoor;
    public Animator rightDoor;
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Open();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Close();
        }
    }

    void Open()
    {
        leftDoor.SetBool("Open", true);
        rightDoor.SetBool("Open", true);
    }

    void Close()
    {
        leftDoor.SetBool("Open", false);
        rightDoor.SetBool("Open", false);
    }
}
