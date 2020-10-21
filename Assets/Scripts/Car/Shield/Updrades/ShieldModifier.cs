using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShieldModifier : CarShield
{
    [SerializeField] protected CarShield modedShield;
    [SerializeField] protected Enemy victim;

    public ShieldModifier(CarShield modifiedShield)
    {
        this.modedShield = modifiedShield;
    }

    public void HitEnemy(Enemy vict, Vector3 force, float damage)
    {
        if (!modedShield.Equals(null))
        {
            victim = vict;
            modify();
        }
    }
    protected abstract void modify();

    public void AddDamage(int bonusDamage)
    {

    }
    ~ShieldModifier()
    {
        modedShield.RemoveShieldModifier(this);
    }

}
