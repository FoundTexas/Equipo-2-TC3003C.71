using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem;

public class PlayerHealth : MonoBehaviour, IDamage
{

    float hp;
    float maxHp = 6;
    float iFrames = 0f;
    public HealthBar healthBar;
    public WeaponManager playerWeapons;
    public AmmoDisplay ammoDisplay;
    public GameObject explosionfx;
    public GameObject forceField;
    Weapon currentWeapon;
    HitStop hitStop;
    SceneLoader sceneLoader;
    
    

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        healthBar.SetMaxHealth(maxHp);
        GameObject manager = GameObject.FindWithTag("Manager");
        if(manager!=null)
            hitStop = manager.GetComponent<HitStop>();
        sceneLoader = GameObject.FindWithTag("SceneLoader").GetComponent<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        currentWeapon = playerWeapons.CurrentSelect();
        if(currentWeapon != null)
        {
            if (ammoDisplay)
            {
                ammoDisplay.SetCurrentAmmo(currentWeapon.curMagazine.ToString());
                ammoDisplay.SetRemainingAmmo(currentWeapon.curAmmo.ToString());
            }
        }
        iFrames -= Time.deltaTime;
        if(iFrames <= 0)
            forceField.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy" && iFrames <= 0)
        {
            TakeDamage(1);
        }
    }   
    
    void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Enemy" && iFrames <= 0)
        {
            TakeDamage(1);
        }
    } 

    public void AddHealth(float amount)
    {
        hp += amount;
        healthBar.SetHealth(hp);
    }

    public WeaponManager GetWeaponManager()
    {
        return playerWeapons;
    }

    public void Die()
    {
        GameObject deathvfx;
        Vector3 vfxpos = this.transform.position;
        vfxpos.y = this.transform.position.y + 1;
        deathvfx = Instantiate(explosionfx, vfxpos, Quaternion.identity);
        
        hitStop.HitStopFreeze(0.2f, 0.1f);
        this.gameObject.SetActive(false);
        sceneLoader.LoadByIndex(1);
        
        var vfxDuration = 1f;
        Destroy(deathvfx, vfxDuration);
        
    }

    public virtual void TakeDamage(float dmg)
    {
        iFrames = 2f;
        hp -= dmg;
        healthBar.SetHealth(hp);
        if (hp <= -1)
        {
            Die();
        }
        else
        {
            forceField.SetActive(true);
            hitStop.HitStopFreeze(0.02f, 0.2f);
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
