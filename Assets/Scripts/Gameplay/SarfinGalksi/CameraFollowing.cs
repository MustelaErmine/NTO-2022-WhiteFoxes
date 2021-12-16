using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] Transform target;

    new Transform transform;
    Vector3 delta;

    void Start()
    {
        transform = GetComponent<Transform>();
        delta = transform.position - target.position;
    }
    private void FixedUpdate()
    {
        
    }
}
