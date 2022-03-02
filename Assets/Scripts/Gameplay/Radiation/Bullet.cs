using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bullet : MonoBehaviour
{
    public Enemy target;
    public Vector3 targetVector;
    public float arr;
    const float timeAlive = 2f;
    bool isTargeted = true;
    DateTime started;
    public void Start()
    {
        print(target);
        print(targetVector);
        started = DateTime.Now;
        if (target == null)
            isTargeted = false;
    }
    void Update()
    {
        if (isTargeted)
        {
            if (target != null)
            {
                if ((target.transform.position - transform.position).magnitude > 2f)
                {
                    transform.Translate((target.transform.position - transform.position).normalized);
                }
                else
                {
                    target.Hurt(arr);
                    Destroy(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if ((DateTime.Now - started).TotalSeconds > timeAlive)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.Translate(targetVector.normalized);
            }
        }
    }
}
