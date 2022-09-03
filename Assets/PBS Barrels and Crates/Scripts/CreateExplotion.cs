using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateExplotion : MonoBehaviour
{
    public GameObject ExplosiveCrate;
    private void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.name == "bullet" || other.CompareTag("Player"))
        {
			Instantiate(ExplosiveCrate, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
	}

}
