using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpBonus : Bonus
{
    [SerializeField] private float bonusAmount = 1f;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<CarShield>() != null)
        {
            other.GetComponentInParent<CarShield>().RegisterShieldModifier(new DamageUpModifier(other.GetComponentInParent<CarShield>(), bonusAmount));
            Destroy(this.gameObject);
        }
    }
}
