using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpBonus : Bonus
{
    [SerializeField] private float bonusAmount = 1f;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("DamageUpBonus, OnTriggerEnter");
        if (other.GetComponentInParent<CarShield>() != null)
        {
            Debug.Log("DamageUpBonus, OnTriggerEnter : CarShield = " + other.GetComponentInParent<CarShield>());   
            other.GetComponentInParent<CarShield>().RegisterShieldModifier(new DamageUpModifier(other.GetComponentInParent<CarShield>(), bonusAmount));
            Destroy(this.gameObject);
        }
    }
}
