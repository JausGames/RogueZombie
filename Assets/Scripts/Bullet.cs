using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private bool hasHit = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponentInParent<Enemy>() != null && !hasHit)
        {
            hasHit = true;
            col.gameObject.GetComponentInParent<Enemy>().GetHitByBullet(col.relativeVelocity);
            return;
        }
    }
}
