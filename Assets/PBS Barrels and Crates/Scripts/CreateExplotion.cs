using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateExplotion : MonoBehaviour
{
    public GameObject ExplosiveCrate;

    public void Expolde()
    {
        Instantiate(ExplosiveCrate, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "bullet" || other.CompareTag("Player"))
        {
            Expolde();
        }
	}

}
