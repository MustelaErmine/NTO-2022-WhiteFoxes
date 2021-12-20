using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConstructionController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public Transform mainShape;
    Vector2 startedAt;
    public bool delete = false;
    public GameObject detail;
    public ItemType detailTypeUse = ItemType.Bonus;
    public ScrollRect scroll;
    public GameObject scrollPat;
    Transform scrollParent;

    public void OnDrag(PointerEventData eventData)
    {
        mainShape.Rotate(mainShape.InverseTransformDirection(eventData.delta.y, -eventData.delta.x, 0f));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startedAt = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if ((eventData.position - startedAt).magnitude < 0.1f)
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            RaycastHit[] hits = Physics.RaycastAll(ray, 1000f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject.name == "Capsule" && detailTypeUse != ItemType.Bonus)
                {
                    CreateDetail(mainShape.InverseTransformPoint(hit.point), 
                        Quaternion.LookRotation(hit.normal).eulerAngles, detailTypeUse);
                }
                else if (delete && hit.transform.gameObject.tag == "Detail")
                {
                    delete = false;
                    Save.instance.session.inventory.Add(hit.transform.GetComponent<Detail>().DetailType);
                    Save.instance.session.shipDetails.Remove(hit.transform.GetComponent<Detail>().reference);
                    Destroy(hit.transform.gameObject);
                    UpdateItems();
                }
            }
        }
    }

    void Start()
    {
        scrollParent = scroll.transform.GetChild(0).GetChild(0);
        UpdateItems();
        ShipDetail[] list = new ShipDetail[Save.instance.session.shipDetails.Count];
        Save.instance.session.shipDetails.CopyTo(list);
        Save.Load();
        foreach (ShipDetail item in list)
        {
            CreateDetail(item.Position, item.Rotation, item.item);
        }
    }

    public void UpdateItems()
    {
        for (int i = 0; i < scrollParent.childCount; i++)
        {
            Destroy(scrollParent.GetChild(i).gameObject);
        }
        foreach (ItemType item in Save.instance.session.inventory)
        {
            if (new List<ItemType> { ItemType.DetailFire, ItemType.DetailIce, ItemType.DetailRadiation }.Contains(item))
            {
                //print(item);
                GameObject itemBtn = Instantiate(scrollPat, scrollParent);
                itemBtn.GetComponentInChildren<Text>().text = item.ToString();
                Button btn = itemBtn.GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    detailTypeUse = item;
                    delete = false;
                });
                foreach (MonoBehaviour component in itemBtn.GetComponents<MonoBehaviour>())
                {
                    component.enabled = true;
                }
                foreach (MonoBehaviour component in itemBtn.GetComponentsInChildren<MonoBehaviour>())
                {
                    component.enabled = true;
                }
            }
        }
    }
    public void ActivateDelete()
    {
        delete = true;
        detailTypeUse = ItemType.Bonus;
    }
    public void CreateDetail(Vector3 position, Vector3 quaternion, ItemType type)
    {
        GameObject obj = Instantiate(detail, mainShape);
        obj.transform.localPosition = position;
        obj.transform.eulerAngles = quaternion;
        Detail detailController = obj.GetComponent<Detail>();
        detailController.DetailType = type;
        detailController.controller = this;
        detailController.Initialize();
    }
    [ContextMenu("Keep")]
    public void Keep()
    {
        Save.Keep();
    }
    public void NextSpace()
    {
        Save.instance.session.NextStep();
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlanetChoice");
    }
}
