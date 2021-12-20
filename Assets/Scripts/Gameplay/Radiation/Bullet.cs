using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Enemy target;
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
                target.Hurt();
                Destroy(gameObject);
            }
        } 
        else
        {
            Destroy(gameObject);
        }
    }
}
