using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamage
{

    float hp;
    float maxHp = 6;
    public HealthBar healthBar;
    public WeaponManager playerWeapons;
    public AmmoDisplay ammoDisplay;
    Weapon currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        healthBar.SetMaxHealth(maxHp);
    }

    // Update is called once per frame
    void Update()
    {
        currentWeapon = playerWeapons.currentSelect();
        if(currentWeapon != null)
        {
            ammoDisplay.SetCurrentAmmo(currentWeapon.curMagazine.ToString());
            ammoDisplay.SetRemainingAmmo(currentWeapon.curAmmo.ToString());
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            TakeDamage(1);
        }
    }   
    
    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            TakeDamage(1);
        }
    } 

    public void AddHealth(float amount)
    {
        hp += amount;
        healthBar.SetHealth(hp);
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    public virtual void TakeDamage(float dmg)
    {
        hp -= dmg;
        healthBar.SetHealth(hp);

        if (hp <= -1)
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
