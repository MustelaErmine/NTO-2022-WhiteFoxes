using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ConstructionController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public Transform mainShape;
    Vector2 startedAt;
    public bool delete = false;
    public GameObject detail;
    public ItemType detailTypeUse = ItemType.Bonus;
    public ScrollRect scroll;
    public GameObject scrollPat;
    public Text[] texts;
    public CaseType[] caseTypes = new CaseType[4] { CaseType.Opened, CaseType.Simple, CaseType.Unsimple, CaseType.Rare};
    Transform scrollParent;
    public RectTransform messageBox, casesPanel;
    public AudioClip notif;
    private Action action;

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
        ShipDetail[] list = new ShipDetail[Save.instance.session.shipDetails.Count];
        Save.instance.session.shipDetails.CopyTo(list);
        Save.Load();
        scrollParent = scroll.transform.GetChild(0).GetChild(0);
        UpdateItems();
        foreach (ShipDetail item in list)
        {
            CreateDetail(item.Position, item.Rotation, item.item);
        }
        UpdateCaseAmounts();
    }

    void UpdateCaseAmounts()
    {
        for (int i = 0; i < 4; i++)
        {
            texts[i].text = Save.instance.session.cases.FindAll((Item item) => item.caseType == caseTypes[i]).Count.ToString();
        }
    }
    public void OpenCaseType(int type)
    {
        Item item = null;
        foreach(Item i in Save.instance.session.cases)
        {
            if (i.caseType == caseTypes[type])
            {
                item = i;
                break;
            }
        }
        if (item != null)
        {
            Save.instance.session.cases.Remove(item);
            ShowMessage("Вам выпал: " + item.caseItem.ItemToString(), ()=> {
                casesPanel.gameObject.SetActive(false);
                Save.instance.session.inventory.Add(item.caseItem);
                UpdateCaseAmounts();
            });
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
            if (new List<ItemType> { ItemType.DetailFire, ItemType.DetailIce, ItemType.DetailRadiation,
                ItemType.AutoFire, ItemType.AutoIce, ItemType.AutoRadiation}.Contains(item))
            {
                GameObject itemBtn = Instantiate(scrollPat, scrollParent);
                itemBtn.GetComponentInChildren<Text>().text = item.ItemToString();
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
    public IEnumerator WaitToButton(Action action)
    {
        while (!Input.GetKeyDown(KeyCode.C))
            yield return null;
        messageBox.gameObject.SetActive(false);
        enabled = true;
        yield return new WaitForSeconds(0.01f);
        action?.Invoke();
    }
    public void ShowMessage(string text, Action action)
    {
        messageBox.gameObject.SetActive(true);
        messageBox.GetChild(1).GetComponent<Text>().text = text;
        messageBox.GetComponent<AudioSource>().PlayOneShot(notif);
        //StartCoroutine(WaitToButton(action));
        this.action = action;
    }
    public void StopWaiting()
    {
        StartCoroutine(StopWaitingCoroutine());
    }
    public IEnumerator StopWaitingCoroutine()
    {
        messageBox.gameObject.SetActive(false);
        enabled = true;
        yield return new WaitForSeconds(0.01f);
        action?.Invoke();
    }
}
