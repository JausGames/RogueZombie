using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class CarShield : MonoBehaviour
{
    [SerializeField] protected string area = "abstract";
    [SerializeField] protected bool isBroken = false;
    [SerializeField] protected float health = 400f;
    [SerializeField] protected Shieldometer shield;
    [SerializeField] List<ShieldModifier> modifiers;
    [SerializeField] protected float damage = 1f;

    public string GetArea()
    {
        return area;
    }
    public bool GetHit(float damage)
    {
        if (health > damage)
        {
            health -= damage;
            shield.SetHealth(health, area);
            return true;
        }
        else if (!isBroken)
        {
            Break();
        }
        return false;
    }

    public void Break()
    {
        isBroken = true;
        health = 0f;
        shield.SetHealth(health, area);
        Debug.Log("The + " + area + " is broken");
    }
    public bool GetBroken()
    {
        return isBroken;
    }
    public void HitEnemy(Enemy victim, Vector3 force)
    {
        var dam = damage;
        for(int i = 0; i < modifiers.Count; i++)
        {
            modifiers[i].HitEnemy(victim, force, dam);
        }
        Debug.Log("CarShield HitEnemy : damage = " + dam);
        victim.GetHitByCar(force, damage);
    }
    public void RegisterShieldModifier(ShieldModifier mod)
    {
        modifiers.Add(mod);
    }
    public void RemoveShieldModifier(ShieldModifier mod)
    {
        modifiers.Remove(mod);
    }
    protected void UpdateChecks()
    {

    }

    public void AddDamage(float extraDamage)
    {
        damage += extraDamage;
    }
    public void RemoveDamage(float malusDamage)
    {
        damage -= malusDamage;
    }
}

