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
    public ItemType DetailType
    {
        set
        {
            _detailType = value;
            Color color;
            switch (_detailType)
            {
                case ItemType.DetailFire:
                    color = Color.red;
                    break;
                case ItemType.DetailIce:
                    color = Color.blue;
                    break;
                case ItemType.DetailRadiation:
                    color = Color.green;
                    break;
                default:
                    throw new System.ArgumentException();
            }
            GetComponentInChildren<MeshRenderer>().material.color = color;
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
            InvokeRepeating("Shot", 2f, 2f);
        }
    }
    void Shot()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType<Enemy>();
        Enemy minenemy = null;
        float mindist = Mathf.Pow(10, 9);
        foreach(Enemy enemy in enemies)
        {
            if (mindist > (transform.position - enemy.transform.position).magnitude)
            {
                mindist = (transform.position - enemy.transform.position).magnitude;
                minenemy = enemy;
            }
        }
        GameObject bul = Instantiate(bullet, transform.position + new Vector3(0, 1, 0), new Quaternion(0,0,0,0));
        bul.GetComponent<Bullet>().target = minenemy;
    }
}
