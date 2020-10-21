using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarColliders : MonoBehaviour
{
    [SerializeField] private Car car;

    [SerializeField] private List<Collider> colliders;
    [SerializeField] private List<WheelCollider> wheelColliders;
    [SerializeField] private Collider headCollider;
    [SerializeField] private Collider tailCollider;
    [SerializeField] private Collider rightCollider;
    [SerializeField] private Collider leftCollider;
    [SerializeField] private List<Collider> roofColliders;

    // Start is called before the first frame update
    void Awake()
    {
        wheelColliders.AddRange(FindObjectsOfType<WheelCollider>());
        headCollider = transform.Find("head").gameObject.GetComponent<Collider>();
        tailCollider = transform.Find("tail").gameObject.GetComponent<Collider>();
        rightCollider = transform.Find("right").gameObject.GetComponent<Collider>();
        leftCollider = transform.Find("left").gameObject.GetComponent<Collider>();
        roofColliders.AddRange(transform.Find("roof").gameObject.GetComponents<Collider>());
        colliders.AddRange(roofColliders);
        colliders.Add(headCollider);
        colliders.Add(tailCollider);
        colliders.Add(rightCollider);
        colliders.Add(leftCollider);
        car = GetComponentInParent<Car>();
    }
    public Transform GetHeadCollider()
    {
        return headCollider.transform;
    }
    public Transform GetTailCollider()
    {
        return tailCollider.transform;
    }
    public List<Collider> GetColliders()
    {
        return colliders;
    }
    public List<WheelCollider> GetWheelColliders()
    {
        return wheelColliders;
    }
}

