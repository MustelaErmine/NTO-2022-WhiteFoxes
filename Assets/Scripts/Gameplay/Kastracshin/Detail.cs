using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Detail : MonoBehaviour
{
    private ItemType _detailType;
    public ConstructionController controller;
    public ShipDetail reference;
    public bool work = false;
    [SerializeField] GameObject bullet;
    [SerializeField] Mesh fMesh, eMesh, rMesh;
    [SerializeField] Material fMat, eMat, rMat;
    [SerializeField] AudioClip fSound, eSound, rSound;
    public Camera cam;
    Vector3 camOldEulers;
    AudioClip mainclip;
    DateTime lastShot = DateTime.MinValue;

    float arr, sec, workRadius;

    const float viewAngle = 30f;
    public ItemType DetailType
    {
        set
        {
            _detailType = value;
            Mesh m = null;
            Material mat = null;
            AudioClip c = null;
            switch (_detailType)
            {
                case ItemType.DetailFire:
                    mat = fMat;
                    m = fMesh;
                    c = fSound;
                    break;
                case ItemType.DetailIce:
                    mat = eMat;
                    m = eMesh;
                    c = eSound;
                    break;
                case ItemType.DetailRadiation:
                    mat = rMat;
                    m = rMesh;
                    c = rSound;
                    break;
                default:
                    throw new System.ArgumentException();
            }
            GetComponentInChildren<MeshRenderer>().material = mat;
            GetComponentInChildren<MeshFilter>().mesh = m;
            mainclip = c;
        }
        get
        {
            return _detailType;
        }
    }

    public void Initialize()
    {
        Save.instance.session.inventory.Remove(DetailType);
        reference = new ShipDetail(transform.localPosition, transform.localEulerAngles, DetailType);
        Save.instance.session.shipDetails.Add(reference);
        //print(Save.instance.session.shipDetails);
        controller.UpdateItems();
        controller.detailTypeUse = ItemType.Bonus;
        cam = GetComponentInChildren<Camera>();
        camOldEulers = cam.transform.eulerAngles;
    }

    public void Update()
    {
        if (work)
        {
            work = false;
            if (_detailType == ItemType.DetailFire)
            {
                arr = 7f; sec = 1f; workRadius = 5f;
            }
            else if (_detailType == ItemType.DetailIce)
            {
                arr = 7f; sec = 2f; workRadius = 5f;
            }
            else if (_detailType == ItemType.DetailRadiation)
            {
                arr = 7f; sec = 1.5f; workRadius = 5f;
            }
            StartCoroutine(Shot());
        }
    }
    IEnumerator Shot()
    {
        while (true)
        {
            yield return new WaitForSeconds(sec);
            Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
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
            if (mindist <= workRadius)
            {
                GameObject bul = Instantiate(bullet, transform.position + new Vector3(0, 1, 0), new Quaternion(0, 0, 0, 0));
                bul.GetComponent<Bullet>().target = minenemy;
                bul.GetComponent<Bullet>().arr = arr;
                GetComponent<AudioSource>().PlayOneShot(mainclip);
            }
        }
    }
    public void MakeShot(Vector3 vector)
    {
        float arr = 0f;
        if (_detailType == ItemType.DetailFire)
            arr = 7;
        else if (_detailType == ItemType.DetailIce)
            arr = 7;
        else if (_detailType == ItemType.DetailRadiation)
            arr = 7;
        if ((DateTime.Now - lastShot).TotalSeconds > sec) {
            //vector = transform.position + vector * 10f;
            GameObject bul = Instantiate(bullet, transform.position + new Vector3(0, 0.5f, 0), new Quaternion(0, 0, 0, 0));
            bul.GetComponent<Bullet>().targetVector = vector;
            bul.GetComponent<Bullet>().arr = arr;
            GetComponent<AudioSource>().PlayOneShot(mainclip);
            lastShot = DateTime.Now;
        }
    }
    public void RotateCamera(Vector3 eulers)
    {
        Vector3 now = cam.transform.eulerAngles + new Vector3(eulers.y, -eulers.x) * 10f;
        Vector3 old = camOldEulers;
        cam.transform.eulerAngles = now;
    }
}
