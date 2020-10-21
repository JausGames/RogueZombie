using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Horde : MonoBehaviour
{
    [SerializeField] private List<EnemyZombie> horde;
    [SerializeField] private float speed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        horde.AddRange(FindObjectsOfType<EnemyZombie>());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach(EnemyZombie zom in horde)
        {
            zom.GetComponent<NavMeshAgent>().speed += 0.01f;
        }
    }
}
