using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float health = 100f;
    public ShipOnPlanet ship;
    public Material normal, hurt, rad;
    public Mesh mnormal, mhurt, mrad;
    void Start()
    {
        StartCoroutine(Walk());
    }
    IEnumerator Walk()
    {
        while (true)
        {
            Vector3 oldpos = transform.position;
            Vector3 newPos = ship.transform.position;
            GetComponent<Rigidbody>().velocity = (newPos - oldpos).normalized;
            yield return null;
        }
    }
    public void Hurt(float arr)
    {
        health -= arr * 2;
        StartCoroutine(Hurted());
        
        if (health < 0f)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator Hurted()
    {
        GetComponent<MeshRenderer>().material = hurt;
        GetComponent<MeshFilter>().mesh = mhurt;
        yield return new WaitForSeconds(1f);
        GetComponent<MeshRenderer>().material = normal;
        GetComponent<MeshFilter>().mesh = mnormal;
    }
}
