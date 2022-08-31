using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    public void Die();
    public void TakeDamage(float dmg);
    public void Freeze();
    public void Burn();
    public IEnumerator Burnning();
}
