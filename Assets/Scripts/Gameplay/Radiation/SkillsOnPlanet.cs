using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillsOnPlanet : MonoBehaviour
{
    ShipOnPlanet ship;
    float hj, hs, en, wv;
    const float hjCuldown = 1f, hsCuldown = 1f, enCuldown = 1f, wvCuldown = 1f;
    [SerializeField] Sprite hjNorm, hjDown, hsNorm, hsDown, enNorm, enDown, wvNorm, wvDown;
    [SerializeField] Image hjImg, hsImg, enImg, wvImg;
    void Start()
    {
        ship = GetComponent(typeof(ShipOnPlanet)) as ShipOnPlanet;
    }
    public void HyperJump()
    {
        if (hj <= 0.01f)
        {
            transform.Translate(transform.TransformDirection(Vector3.forward) * 10f);
            hj = hjCuldown;
        }
    }
    public void HighSpeed()
    {
        if (hs <= 0.01f)
        {
            StartCoroutine(SpeedUpCoroutine());
            hs = hsCuldown;
        }
    }
    public IEnumerator SpeedUpCoroutine()
    {
        Time.timeScale = 0.75f;
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 1f;
    }
    public void Energy()
    {
        if (en <= 0.01f)
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                if ((enemy.transform.position - transform.position).magnitude < 10f)
                {
                    enemy.Hurt(20f);
                }
            }
            en = enCuldown;
        }
    }
    public void Wave()
    {
        if (wv <= 0.01f)
        {
            Enemy[] enemies = FindObjectsOfType<Enemy>();
            foreach (Enemy enemy in enemies)
            {
                if ((enemy.transform.position - transform.position).magnitude < 10f)
                {
                    enemy.GetComponent<Rigidbody>().AddForce((enemy.transform.position - transform.position) * 1000f);
                }
            }
            wv = wvCuldown;
        }
    }

    private void Update()
    {
        hs -= Time.deltaTime * 0.1f;
        hj -= Time.deltaTime * 0.1f;
        en -= Time.deltaTime * 0.1f;
        wv -= Time.deltaTime * 0.1f;
        hs = Mathf.Max(hs, 0);
        hj = Mathf.Max(hj, 0);
        en = Mathf.Max(en, 0);
        wv = Mathf.Max(wv, 0);
        if (hs <= 0.01f)
        {
            hsImg.sprite = hsNorm;
        }
        else
        {
            hsImg.sprite = hsDown;
        }
        if (hj <= 0.01f)
        {
            hjImg.sprite = hjNorm;
        }
        else
        {
            hjImg.sprite = hjDown;
        }
        if (en <= 0.01f)
        {
            enImg.sprite = enNorm;
        }
        else
        {
            enImg.sprite = enDown;
        }
        if (wv <= 0.01f)
        {
            wvImg.sprite = wvNorm;
        }
        else
        {
            wvImg.sprite = wvDown;
        }
    }
}
