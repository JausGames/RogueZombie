using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyZombie : Enemy
{
    [SerializeField] private Car player;
    [SerializeField] private Transform spine;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float coolDown = 0.1f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private const float STAND_UP_TIME = 3f;
    [SerializeField] private  float standUpTime;
    [SerializeField] private bool ragdoll = false;
    [SerializeField] private bool dead = false;
    [SerializeField] private bool isClosed = false;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private ZombieAnimatorController anim;
    [SerializeField] private Collider[] collids;
    [SerializeField] private Rigidbody[] bodies;
    [SerializeField] Sprite skinArm;
    [SerializeField] Sprite clothArm;
    [SerializeField] Sprite clothBody;
    [SerializeField] Sprite clothLegs;

    // Start is called before the first frame update
    void Start()
    {
        health = 200f;
        collids = GetComponentsInChildren<Collider>();
        bodies = GetComponentsInChildren<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Car>();
        anim = GetComponentInChildren<ZombieAnimatorController>();

        var colliders = FindObjectOfType<CarColliders>();
        var colsw = colliders.GetWheelColliders();
        foreach (Collider col in colsw)
        {
            foreach (Collider coll in collids)
            {
                Debug.Log("Die, kill collider : collider = " + col);
                Physics.IgnoreCollision(col, coll, true);
            }
        }
    }
    private void Update()
    {
        if (Time.time > standUpTime && ragdoll && !dead)
        {
            transform.position = spine.position;
            agent.enabled = true;
            anim.SetAnimOn(true);
            ragdoll = false;

            var colliders = FindObjectOfType<CarColliders>();
            var cols = colliders.GetColliders();
            foreach (Collider col in cols)
            {
                foreach (Collider coll in collids)
                {
                    Debug.Log("Die, kill collider : collider = " + col);
                    Physics.IgnoreCollision(col, coll, false);
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var wasClosed = isClosed;
        var wasAttacking = isAttacking;
        isClosed = false;
        Collider[] hitColliders = Physics.OverlapCapsule(transform.position - transform.up, transform.position + transform.up, 0.7f);
        isAttacking = false;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.GetComponentInParent<Car>() != null)
            {
                if (hitCollider.gameObject.GetComponent<CarShield>() == null) return;
                isClosed = true;
                Debug.Log("Zombie Attack here : " + hitCollider.gameObject.GetComponent<CarShield>().GetArea());
                isAttacking = true;
                if (attackCooldown < Time.time)
                {
                    hitCollider.gameObject.GetComponent<CarShield>().GetHit(damage);
                    if (hitCollider.gameObject.GetComponentInParent<Car>().GetAlive()
                        && hitCollider.gameObject.GetComponent<CarShield>().GetBroken()) hitCollider.gameObject.GetComponentInParent<Car>().removeLife(damage);
                    attackCooldown = Time.time + coolDown;
                }
            }
        }
        if (isAttacking != wasAttacking) anim.SetAttack(isAttacking);
        if (!agent.enabled) return;
        anim.SetWalk(!isClosed);
        if (isClosed != wasClosed) agent.isStopped = isClosed;
        agent.SetDestination(player.transform.position);

    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position - transform.up, 0.7f);
        Gizmos.DrawWireSphere(transform.position + transform.up, 0.7f);
        Gizmos.DrawLine(transform.position - transform.up, transform.position + transform.up);
    }
    public override void GetHitByBullet(Vector3 dir)
    {
        base.GetHitByBullet(dir);
        anim.SetAnimOn(false);
        standUpTime = Time.time + STAND_UP_TIME;
        ragdoll = true;
    }
    public override void GetHitByCar(Vector3 dir, float damage)
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
            spine.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);
        };

        anim.SetAnimOn(false);
        standUpTime = Time.time + STAND_UP_TIME;
        ragdoll = true;

        var colliders = FindObjectOfType<CarColliders>();
        var cols = colliders.GetColliders();
        /*foreach (Collider col in cols)
        {
            foreach (Collider coll in collids)
            {
                Debug.Log("Die, kill collider : collider = " + col);
                Physics.IgnoreCollision(col, coll, true);
            }
        }*/
    }
    protected override void Die()
    {
        health = 0f;
        agent.enabled = false;
        dead = true;

    }
    public void SetColor(Color color, bool value)
    {
        var maker = FindObjectOfType<SpriteMaker>();
        var rends = GetComponentsInChildren<SkinnedMeshRenderer>();
        for (var i = 0; i < rends.Length; i++)
        {

            if (rends[i].gameObject.name == "ZombieArm")
            {
                rends[i].material.mainTexture = maker.OverwriteSprite(skinArm, maker.ColorSprite(clothArm, color));
            }
            if (rends[i].gameObject.name == "ZombieBody")
            {
                rends[i].material.mainTexture = maker.ColorSprite(clothBody, color).texture;
            }
            if (rends[i].gameObject.name == "ZombieLeg" && value)
            {
                rends[i].material.mainTexture = clothLegs.texture;
            }
        }
    }
    public void SetPants()
    {
        var rends = GetComponentsInChildren<SkinnedMeshRenderer>();
        for (var i = 0; i < rends.Length; i++)
        {
            if (rends[i].gameObject.name == "ZombieLeg")
            {
                rends[i].material.mainTexture = clothLegs.texture;
            }
        }
    }

    void SetRigidbodyState(bool state)
    {

    }
}
