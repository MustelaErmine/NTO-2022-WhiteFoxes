using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveElement : MonoBehaviour
{
    DateTime started;
    const float secAlive = 5f;
    void Start()
    {
        started = DateTime.Now;
    }
    private void FixedUpdate()
    {
        transform.Translate(transform.TransformDirection(Vector3.forward) * -1f);
    }
    private void Update()
    {
        if ((DateTime.Now - started).TotalSeconds > secAlive) {
            Destroy(gameObject);
        }
    }
}
