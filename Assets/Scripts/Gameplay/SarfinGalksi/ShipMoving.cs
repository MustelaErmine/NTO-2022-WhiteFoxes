using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipMoving : MonoBehaviour
{
    public new Rigidbody rigidbody;
    public new Transform transform;
    Vector3 oldVelocity = new Vector3();
    [SerializeField] float upWeight = 0;
    [SerializeField] float downWeight = 0;
    [SerializeField] float rightWeight = 0;
    [SerializeField] float leftWeight = 0;

    const float vecticalEngineStep = Mathf.PI / 200;
    const float verticalImpulseStep = Mathf.PI / 100;
    const float horizontalEngineStep = Mathf.PI / 200;
    const float horizontalImpulseStep = Mathf.PI / 100;

    const float maxVerticalSpeed = Mathf.PI / 2;
    const float maxHorizontalSpeed = Mathf.PI / 2;

    Vector3 speed = Vector3.forward * 150f; 

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        Vector3 new_velocity = oldVelocity;
        Vector3 new_angular_velocity = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            upWeight += vecticalEngineStep * Time.deltaTime * 60;
            upWeight = Mathf.Min(maxHorizontalSpeed, upWeight);
        }
        else
        {
            upWeight -= verticalImpulseStep * Time.deltaTime * 60;
            upWeight = Mathf.Max(0, upWeight);
        }

        if (Input.GetKey(KeyCode.S))
        {
            downWeight += vecticalEngineStep * Time.deltaTime * 60;
            downWeight = Mathf.Min(maxVerticalSpeed, downWeight);
        }
        else
        {
            downWeight -= verticalImpulseStep * Time.deltaTime * 60;
            downWeight = Mathf.Max(0, downWeight);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rightWeight += horizontalEngineStep * Time.deltaTime * 60;
            rightWeight = Mathf.Min(maxHorizontalSpeed, rightWeight);
        }
        else
        {
            rightWeight -= horizontalImpulseStep * Time.deltaTime * 60;
            rightWeight = Mathf.Max(0, rightWeight);
        }

        if (Input.GetKey(KeyCode.A))
        {
            leftWeight += horizontalEngineStep * Time.deltaTime * 60;
            leftWeight = Mathf.Min(maxHorizontalSpeed, leftWeight);
        }
        else
        {
            leftWeight -= horizontalImpulseStep * Time.deltaTime * 60;
            leftWeight = Mathf.Max(0, leftWeight);
        }

        //print($"{upWeight}, {downWeight}, { rightWeight}, {leftWeight}");
        new_angular_velocity += Vector3.Lerp(Vector3.zero, transform.TransformDirection(
            new Vector3(-maxVerticalSpeed, 0, 0)), Mathf.Sin(upWeight));
        new_angular_velocity += Vector3.Lerp(Vector3.zero, transform.TransformDirection(
            new Vector3(maxVerticalSpeed, 0, 0)), Mathf.Sin(downWeight));
        new_angular_velocity += Vector3.Lerp(Vector3.zero, transform.TransformDirection(
            new Vector3(0, maxHorizontalSpeed, 0)), Mathf.Sin(rightWeight));
        new_angular_velocity += Vector3.Lerp(Vector3.zero, transform.TransformDirection(
            new Vector3(0, -maxHorizontalSpeed, 0)), Mathf.Sin(leftWeight));
        rigidbody.angularVelocity = new_angular_velocity;

        rigidbody.velocity = transform.TransformDirection(speed);
    }
}
