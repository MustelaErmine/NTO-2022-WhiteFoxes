using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float health = 45f;
    float timer = 1f;
    public ShipOnPlanet ship;
    public Material normal, hurt, rad;
    public AudioClip deathClip;
    public Mesh mnormal, mhurt, mrad;
    [SerializeField] AudioSource death;
    void Start()
    {
        StartCoroutine(Walk());
        transform.GetChild(0).gameObject.SetActive(true);
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
        health -= arr;
        StartCoroutine(Hurted());
        
        if (health < 0f)
        {
            death.PlayOneShot(deathClip);
            if (Random.value > 0.5)
                Save.instance.session.gold += 10;
            Destroy(gameObject);
        }
    }
    IEnumerator Hurted()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material = hurt;
        transform.GetChild(0).GetComponent<MeshFilter>().mesh = mhurt;
        yield return new WaitForSeconds(1f);
        transform.GetChild(0).GetComponent<MeshRenderer>().material = normal;
        transform.GetChild(0).GetComponent<MeshFilter>().mesh = mnormal;
    }
    private void Update()
    {
        timer = Mathf.Min(1f, timer + 1f * Time.deltaTime);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && 1f - timer < 1e-5 && transform.childCount == 1)
        {
            other.GetComponent<ShipOnPlanet>().ApplyRadiation(15f);
            timer = 0f;
        }
    }
}
