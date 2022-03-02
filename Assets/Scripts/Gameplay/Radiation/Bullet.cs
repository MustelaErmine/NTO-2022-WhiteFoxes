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
            Enemy nearest = NearestEnemy();
            if (nearest && (nearest.transform.position - transform.position).magnitude < 2f)
            {
                nearest.Hurt(arr);
                Destroy(gameObject);
            }
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
    Enemy NearestEnemy()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        Enemy minenemy = null;
        float mindist = 1e9f;
        foreach (Enemy enemy in enemies)
        {
            if (mindist > (transform.position - enemy.transform.position).magnitude)
            {
                mindist = (transform.position - enemy.transform.position).magnitude;
                minenemy = enemy;
            }
        }
        return minenemy;
    }
}
