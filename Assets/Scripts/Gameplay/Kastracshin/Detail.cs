using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Detail : MonoBehaviour
{
    private ItemType _detailType;
    public ConstructionController controller;
    public ShipDetail reference;
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
}
