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
    [SerializeField] private Collider[] ragdollCollids;
    [SerializeField] private Collider mainCollid;
    [SerializeField] private GameObject rig;
    [SerializeField] private Rigidbody[] bodies;
    [SerializeField] Sprite skinArm;
    [SerializeField] Sprite skinLeg;
    [SerializeField] Sprite clothArm;
    [SerializeField] Sprite clothBody;
    [SerializeField] Sprite clothLegs;
    // Don't set this too high, or NavMesh.SamplePosition() may slow down
    float onMeshThreshold = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        health = 200f;
        collids = GetComponentsInChildren<Collider>();
        bodies = GetComponentsInChildren<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        player = FindObjectOfType<Car>();
        anim = GetComponentInChildren<ZombieAnimatorController>();
        body = GetComponent<Rigidbody>();

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
        ragdollCollids = rig.GetComponentsInChildren<Collider>();
        foreach (Collider col in ragdollCollids)
        {
            Physics.IgnoreCollision(col, mainCollid, true);
            col.GetComponent<Rigidbody>().isKinematic = true;
            col.enabled = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var wasClosed = isClosed;
        var wasAttacking = isAttacking;
        isClosed = false;
        Collider[] hitColliders = Physics.OverlapCapsule(transform.position - transform.up, transform.position + transform.up * 2f, 0.8f);
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
        if (Time.time > standUpTime && ragdoll && !dead)
        {
            EnableRagdoll(false);
        }
        if (!agent.enabled || !IsAgentOnNavMesh()) return;
        if (isAttacking != wasAttacking) anim.SetAttack(isAttacking);
        anim.SetWalk(!isClosed);
        if (isClosed != wasClosed) agent.isStopped = isClosed;
        if (!ragdoll && !dead && !isClosed && !isAttacking && IsAgentOnNavMesh()) agent.SetDestination(player.transform.position);


    }


    public override void GetHitByBullet(Vector3 dir)
    {
        if (agent.enabled) agent.isStopped = true;
        base.GetHitByBullet(dir);
        if (!ragdoll) EnableRagdoll(true);
        else standUpTime = Time.time + STAND_UP_TIME;
    }

    public override void GetHitByCar(Vector3 dir, float damage)
    {
        if (agent.enabled && IsAgentOnNavMesh()) agent.isStopped = true;
        var magn = new Vector3(dir.x, 0f, dir.z);
        Debug.Log("GetHitByCar, magnitude = " + magn.magnitude);
        if (magn.magnitude < 2f) return;
        if (!GetDamaged(damage * magn.magnitude))
        {
            Debug.Log("EnemyZombie, GetHitByCar : The dude died");
        };
        if (!ragdoll) EnableRagdoll(true);
        else standUpTime = Time.time + STAND_UP_TIME;

    }
    protected override void Die()
    {
        health = 0f;
        agent.enabled = false;
        dead = true;

    }
    public void EnableRagdoll(bool value)
    {
        anim.SetAnimOn(!value);
        agent.enabled = !value;
        ragdoll = value;
        mainCollid.enabled = !value;
        foreach (Collider col in ragdollCollids)
        {
            col.GetComponent<Rigidbody>().isKinematic = !value;
            col.enabled = value;
        }
        //body.isKinematic = value;
        if (value)
        {
            spine.position = agent.transform.position + Vector3.up;
            standUpTime = Time.time + STAND_UP_TIME;
        }
        else
        {
            if (transform.position.y < 0f) transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
            body.velocity = Vector3.zero;
            agent.Warp(spine.position);
        }
    }

    //  VISUAL SETUP
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

    public bool IsAgentOnNavMesh()
    {
        Vector3 agentPosition = agent.transform.position;
        NavMeshHit hit;

        // Check for nearest point on navmesh to agent, within onMeshThreshold
        if (NavMesh.SamplePosition(agentPosition, out hit, onMeshThreshold, NavMesh.AllAreas))
        {
            // Check if the positions are vertically aligned
            if (Mathf.Approximately(agentPosition.x, hit.position.x)
                && Mathf.Approximately(agentPosition.z, hit.position.z))
            {
                // Lastly, check if object is below navmesh
                return agentPosition.y >= hit.position.y;
            }
        }

        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spine.position, 0.2f);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.up, 0.2f);
    }
}
