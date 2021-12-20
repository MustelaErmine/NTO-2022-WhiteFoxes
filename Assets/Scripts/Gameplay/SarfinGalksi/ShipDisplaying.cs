using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipDisplaying : MonoBehaviour
{
    [SerializeField] GameObject detail;
    public bool work = false;
    void Start()
    {
        Save.Load();
        ShipDetail[] list = new ShipDetail[Save.instance.session.shipDetails.Count];
        Save.instance.session.shipDetails.CopyTo(list);
        foreach (ShipDetail item in list)
        {
            CreateDetail(item.Position, item.Rotation, item.item);
        }
    }
    public void CreateDetail(Vector3 position, Vector3 quaternion, ItemType type)
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
    }
}
