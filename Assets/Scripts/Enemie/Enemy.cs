using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

abstract public class Enemy : MonoBehaviour
{
    [SerializeField] protected float health;
    protected NavMeshAgent agent;
    protected Rigidbody body;
    virtual public void GetHitByCar(Vector3 dir, float damage)
    {
        var magn = new Vector3(dir.x, 0f, dir.z);
        Debug.Log("GetHitByCar, magnitude = " + magn.magnitude);
        if (magn.magnitude < 4f) return;
        var force = (-dir + Vector3.up * 5f) * 10f;
        force = Vector3.up * 30f;
        if (!GetDamaged(damage * magn.magnitude))
        {
            var body = GetComponentInChildren<Rigidbody>();
            //foreach (Rigidbody body in bodies) 
            body.AddForce(force, ForceMode.Impulse);
        };
        
    }
    virtual protected void Die()
    {
        health = 0f;
        body.constraints = RigidbodyConstraints.None;
        agent.enabled = false;
    }
    public bool GetDamaged(float damage)
    {
        Debug.Log("Ennemy, GetDamaged : damage = " + damage);
        if (health > damage) {health -= damage; return true; }
        else { Die(); return false; }
    }
    virtual public void GetHitByBullet(Vector3 dir)
    {
        Debug.Log("Get Fucked by bullet BITCHIIIIIIIIIIES");
        Die();
    }
}
