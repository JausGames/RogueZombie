using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageUpModifier : ShieldModifier
{
    [SerializeField] private float bonusDamage; 


    public DamageUpModifier(CarShield modifiedShield, float bonusDamage) : base(modifiedShield) {
        this.bonusDamage = bonusDamage;
        modedShield.AddDamage(bonusDamage);
    }
    protected override void modify()
    {
        Debug.Log("Damage Up Modifier Shield : modify()");
    }
    ~DamageUpModifier()
    {
        modedShield.RemoveDamage(bonusDamage);
        modedShield.RemoveShieldModifier(this);
    }

}
