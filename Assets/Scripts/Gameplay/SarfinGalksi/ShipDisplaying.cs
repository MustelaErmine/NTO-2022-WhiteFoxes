using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDisplaying : MonoBehaviour
{
    [SerializeField] GameObject detail;
    ShipOnPlanet ship;
    public bool work = false;
    void Awake()
    {
        Save.Load();
        ShipDetail[] list = new ShipDetail[Save.instance.session.shipDetails.Count];
        Save.instance.session.shipDetails.CopyTo(list);
        List<Detail> details = new List<Detail>();
        foreach (ShipDetail item in list)
        {
            details.Add(CreateDetail(item.Position, item.Rotation, item.item));
        }
        ship = GetComponent<ShipOnPlanet>();
        if (ship != null)
        {
            ship.details = details.ToArray();
            /*
            foreach(Detail d in ship.details)
            {
                print(d.reference.item.ItemToString());
                print(d.DetailType.ItemToString());
            }*/
        }
    }
    public Detail CreateDetail(Vector3 position, Vector3 quaternion, ItemType type)
    {
        GameObject obj = Instantiate(detail, transform);
        obj.transform.localPosition = new Vector3(position.z, position.y, -position.x);
        obj.transform.eulerAngles = new Vector3(quaternion.x, quaternion.y + 90f, quaternion.z);
        //obj.transform.eulerAngles = quaternion;
        Detail detailController = obj.GetComponent<Detail>();
        detailController.DetailType = type;
        if (work)
        {
            detailController.work = true;
        }
        return detailController;
    }
}
