using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCollidingBehavior : MonoBehaviour

{
    private Car car;
    private float RESISTANCE;
    [SerializeField] private HeadShield head;
    [SerializeField] private LeftBodyShield left;
    [SerializeField] private RightBodyShield right;
    [SerializeField] private TailShield tail;
    [SerializeField] private RoofShield roof;

    private void Awake()
    {
        car = GetComponent<Car>();
    }
    private void Start()
    {

        RESISTANCE = car.GetResistance();
    }
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponentInParent<Enemy>() != null)
        {
            Debug.Log("OnCollisionEnter, thisCollider.name = " + col.GetContact(0).thisCollider.name);
            //col.gameObject.GetComponent<Enemy>().GetHitByCar(col.relativeVelocity);
            if (col.GetContact(0).thisCollider.GetComponent<CarShield>() != null)
            {
                col.GetContact(0).thisCollider.GetComponent<CarShield>().HitEnemy(col.gameObject.GetComponentInParent<Enemy>(), col.relativeVelocity);
            }
            return;
        }
        if ((col.GetContact(0).thisCollider.name != "head" 
            || col.gameObject.tag == "Ground")
            && col.GetContact(0).thisCollider.GetComponent<CarShield>() != null)
        {
            var shield = col.GetContact(0).thisCollider.GetComponent<CarShield>();
            var amount = 10f * col.relativeVelocity.magnitude;
            amount = Mathf.Clamp(amount, 50f, 1000f);
            amount -= 50f;
            amount = amount * 200f / (RESISTANCE + 200f);
            if (amount > 0 && !shield.GetBroken())
            {
                var kill = shield.GetHit(amount);
                if (!kill)
                {
                    Debug.Log("Shield broke");
                }
            }
            else if (amount > 0 && shield.GetBroken())
            {
                var kill = car.removeLife(amount);
                if (!kill)
                {
                    Debug.Log("You Died");
                }
            }
        }
    }
}
