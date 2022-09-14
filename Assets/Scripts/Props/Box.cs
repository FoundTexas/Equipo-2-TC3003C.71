using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour, IDamage
{
    [SerializeField] float hp = 20;
    float maxhp;
    Renderer render;
    public GameObject ExplosiveCrate;
    private void Start()
    {
        maxhp = hp;
        render = GetComponent<Renderer>();
    }
    public void Die()
    {
        GetComponent<Dropper>().Spawn();
        Instantiate(ExplosiveCrate, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void TakeDamage(float dmg)
    {
        hp -= dmg;
        render.material.color = new Color(1, hp / maxhp, hp / maxhp);

        if(hp < 0)
        {
            Die();
        }
    }

    public void Freeze()
    {
        throw new System.NotImplementedException();
    }

    public void Burn()
    {
        throw new System.NotImplementedException();
    }

    public IEnumerator Burnning()
    {
        throw new System.NotImplementedException();
    }
}
