using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Walk());
    }
    IEnumerator Walk()
    {
        while (true)
        {
            Vector3 oldpos = transform.position;
            Vector3 newPos = transform.position;
            float type = Random.value;
            if (type <= 0.25f)
            {
                newPos += new Vector3(10f, 0, 0);
            }
            else if (type <= 0.5f)
            {
                newPos += new Vector3(-10f, 0, 0);
            }
            else if (type <= 0.75f)
            {
                newPos += new Vector3(0, 0, 10f);
            }
            else
            {
                newPos += new Vector3(0, 0, -10);
            }
            while ((transform.position - newPos).magnitude > 1f)
            {
                GetComponent<Rigidbody>().velocity = (newPos - oldpos).normalized;
                yield return null;
            }
        }
    }
}
