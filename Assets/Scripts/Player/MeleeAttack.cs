using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    public float coolDownTime = 0.8f; 
    private int comboClicks = 0;
    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) { Attack(); }
    }

    private void Attack()
    {
        StopAllCoroutines();
        StartCoroutine(ResetCombo());
        comboClicks++;
        comboClicks = Mathf.Clamp(comboClicks, 0, 3);

        if(comboClicks == 1)
            animator.SetTrigger("Attack1");
        if(comboClicks == 2)
            animator.SetTrigger("Attack2");
        if(comboClicks == 3)
            animator.SetTrigger("Attack3");
    }

    private IEnumerator ResetCombo()
    {
        yield return new WaitForSeconds(coolDownTime);
        comboClicks = 0;
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
    }
}
