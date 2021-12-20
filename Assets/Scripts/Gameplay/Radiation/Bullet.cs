using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Enemy target;
    public float arr;
    void Update()
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
}
