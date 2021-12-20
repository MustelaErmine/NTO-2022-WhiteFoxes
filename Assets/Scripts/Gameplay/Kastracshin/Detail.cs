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
    AudioClip mainclip;
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
    }

    public void Update()
    {
        if (work)
        {
            work = false;
            if (_detailType == ItemType.DetailFire)
                StartCoroutine(Shot(2f, 2f));
            else if (_detailType == ItemType.DetailIce)
                StartCoroutine(Shot(2f, 2f));
            else if (_detailType == ItemType.DetailRadiation)
                StartCoroutine(Shot(2f, 1.5f));
        }
    }
    IEnumerator Shot(float arr, float sec)
    {
        while (true)
        {
            yield return new WaitForSeconds(sec);
            Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
            Enemy minenemy = null;
            float mindist = Mathf.Pow(10, 9);
            foreach (Enemy enemy in enemies)
            {
                if (mindist > (transform.position - enemy.transform.position).magnitude)
                {
                    mindist = (transform.position - enemy.transform.position).magnitude;
                    minenemy = enemy;
                }
            }
            GameObject bul = Instantiate(bullet, transform.position + new Vector3(0, 1, 0), new Quaternion(0, 0, 0, 0));
            bul.GetComponent<Bullet>().target = minenemy;
            bul.GetComponent<Bullet>().arr = arr;
            GetComponent<AudioSource>().PlayOneShot(mainclip);
        }
    }
}
