using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBonus : Bonus
{
    [SerializeField] private float bonusAmount = 100f;
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HealBonus, OnTriggerEnter");
        if (other.GetComponentInParent<Car>() != null)
        {
            Debug.Log("HealBonus, OnTriggerEnter : Car = " + other.GetComponentInParent<Car>());   
            other.GetComponentInParent<Car>().HealLife(bonusAmount);
            Destroy(this.gameObject);
        }
    }
}
