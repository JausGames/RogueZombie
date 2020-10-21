using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimatorController : MonoBehaviour
{
    
    [SerializeField] private Animator animator;
    public void SetWalk(bool value)
    {
        animator.SetBool("Walk", value);
        if (value) animator.SetTrigger("WalkTrigger");
    }
    public void SetAttack(bool value)
    {
        animator.SetBool("Attack", value);
        if (value) animator.SetTrigger("AttackTrigger");
    }
    public void SetAnimOn(bool value)
    {
        animator.enabled = value;
    }

}
