using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowing : MonoBehaviour
{
    [SerializeField] Transform target;

    new Transform transform;
    Vector3 delta;
    Quaternion quaternionDelta;

    const float speed = 1f;

    void Start()
    {
        transform = GetComponent<Transform>();
        delta = target.transform.InverseTransformPoint(transform.position);
        //quaternionDelta = Quaternion.FromToRotation(transform.position, target.TransformPoint(Vector3.up * 1.2f));
    }
    private void Update()
    {
        Vector3 moveTo = target.TransformPoint(delta);
        transform.position = Vector3.MoveTowards(transform.position, moveTo, speed);

        //Quaternion ftr = quaternionDelta.Add(transform.rotation);
        transform.LookAt(target.position + target.up * 1.7f);
    }
}
