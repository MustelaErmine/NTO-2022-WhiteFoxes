using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowToTarget : MonoBehaviour
{
    [SerializeField] Transform target;
    new Transform transform;

    private void Start()
    {
        transform = GetComponent<Transform>();
    }
    void Update()
    {
        transform.LookAt(target);
    }
}
