using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Construction : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    bool _deleteMode = false;
    public bool DeleteMode
    {
        get => _deleteMode;
        set
        {
            _deleteMode = value;
        }
    }

    public Transform mainShape;
    Vector2 startedAt;

    [SerializeField] GameObject blockPrefab;
    [SerializeField] RectTransform scrollParent;
    [SerializeField] GameObject scrollPat;
    [SerializeField] Transform particle;
    [SerializeField] Button cont;
    [SerializeField] RectTransform messageBox;
    [SerializeField] AudioClip notif;

    [SerializeField] Text[] texts;
    [SerializeField] CaseType[] caseTypes;
    [SerializeField] Image casesPanel;

    Ship ship;
    Action action;

    Vector3 offset = new Vector3(0, 0, 5f);
    (ShipBlockDisplay, int, int, Vector3, Vector3) clayInfo;
    public const float range = 0.5f;
    private const float dest = 0.25f;
    bool freeBlock = false;
    ShipBlockDisplay nowDisplay;

    private void Start()
    {
        Save.Load();
        if (Save.instance.session == null)
        {
            Save.instance.session = new Session();
            Save.instance.session.inventory.Add(ItemType.Water);
            Save.instance.session.inventory.Add(ItemType.Food);
            Save.instance.session.inventory.Add(ItemType.Fuel);

            Save.instance.session.inventory.Add(ItemType.GunModule);
            Save.instance.session.inventory.Add(ItemType.GunModule);
            Save.instance.session.inventory.Add(ItemType.Engine);
            Save.instance.session.inventory.Add(ItemType.Engine);
        }
        if (Save.instance.session.ship != null)
        {
            ship = Save.instance.session.ship;
        }
        else
        {
            ship = new Ship(true);
        }
        UpdateShip();
        UpdateItems(); 
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
        foreach (Item i in Save.instance.session.cases)
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
            ShowMessage("Вам выпал: " + item.caseItem.ItemToString(), () => {
                casesPanel.gameObject.SetActive(false);
                Save.instance.session.inventory.Add(item.caseItem);
                UpdateCaseAmounts();
            });
        }
    }

    void UpdateShip()
    {
        ShipBlockDisplay[] blocks = FindObjectsOfType<ShipBlockDisplay>();
        foreach (ShipBlockDisplay block in blocks)
        {
            Destroy(block.gameObject);
        }
        List<ShipBlockDisplay> bl = new List<ShipBlockDisplay>();
        //ship.blocks.ToArray().Print();
        foreach (var block in ship.blocks)
        {
            print(block.ToString());
            ShipBlockDisplay display = Instantiate(blockPrefab).GetComponent<ShipBlockDisplay>();
            display.dest = 0.25f;
            display.constructed = true;
            display.block = block;
            display.offset = offset;
            if (block.parent != -1)
            {
                foreach (var b in bl)
                {
                    if (b.block.number == block.parent)
                    {
                        display.transform.parent = b.transform;
                        break;
                    }
                }
            }
            else
            {
                mainShape = display.transform;
            }

            bl.Add(display);
        }
        UpdateContinueButton();
    }

    public void OnDrag(PointerEventData eventData)
    {
        /*if ((eventData.position - startedAt).magnitude < 0.1f)
        {*/
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000f);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.GetComponent<ShipBlockDisplay>())
            {
                var display = hit.transform.gameObject.GetComponent<ShipBlockDisplay>();
                if (display.constructed)
                {
                    mainShape.Rotate(mainShape.InverseTransformDirection(eventData.delta.y, -eventData.delta.x, 0f));
                }
                else
                {
                    hit.transform.Translate(new Vector3(eventData.delta.x / Screen.width, eventData.delta.y / Screen.height, 0f) * 1.8f);
                    (ShipBlockDisplay, int, int, Vector3, Vector3) data = FindThunder(display);
                    //print($"{data.Item1.block.type.ToString()}, {data.Item2}, {data.Item3}, {(data.Item4 - data.Item5).magnitude}");
                    Vector3 from = data.Item5;
                    Vector3 to = data.Item4;
                    clayInfo = data;
                    if ((from - to).magnitude <= range)
                    {
                        //print("ddd");
                        particle.transform.position = from;
                        Vector3 direction = to - from;
                        particle.transform.LookAt(to);
                        //particle.transform.eulerAngles = new Vector3(0, 0, Mathf.Atan(direction.y / direction.x));
                        //particle.transform.localScale = new Vector3(1, (from - to).magnitude * 0.2f, 1); 
                    }
                    else
                    {
                        particle.transform.position = new Vector3(25, 0, 0);
                    }
                }
            }
        }
        //}
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(clayInfo.Item4, clayInfo.Item5);
    }

    public (ShipBlockDisplay, int, int, Vector3, Vector3) FindThunder(ShipBlockDisplay display)
    {
        //print("");
        Vector3 forw = new Vector3(0,0,-1), back = new Vector3(0,0,1), right = new Vector3(-1,0,0), left = new Vector3(1,0,0),
            up = new Vector3(0,1,0), down = new Vector3(0,-1,0);
        float min_dest = 1e9f;
        ShipBlockDisplay nd = null;
        int where_from = 0, where_to = 0;
        Vector3 from_z = Vector3.zero, to_z = Vector3.zero;
        Vector3[] vectors = new Vector3[] { forw, back, up, down, right, left };
        foreach (var other in FindObjectsOfType<ShipBlockDisplay>())
        {
            if (other != display)
            {
                for (int i = 0; i < 6; i++)
                {
                    int j = 0;
                    if (i == 0)
                        j = 1;
                    if (i == 1)
                        j = 0;
                    if (i == 2)
                        j = 3;
                    if (i == 3)
                        j = 2;
                    if (i == 4)
                        j = 5;
                    if (i == 5)
                        j = 4;
                    if (i == 0 && other.forw.gameObject.activeSelf && display.back.gameObject.activeSelf ||
                        i == 1 && other.back.gameObject.activeSelf && display.forw.gameObject.activeSelf ||
                        i == 2 && other.up.gameObject.activeSelf && display.down.gameObject.activeSelf ||
                        i == 3 && other.down.gameObject.activeSelf && display.up.gameObject.activeSelf ||
                        i == 4 && other.right.gameObject.activeSelf && display.left.gameObject.activeSelf ||
                        i == 5 && other.left.gameObject.activeSelf && display.right.gameObject.activeSelf)
                    {
                        Vector3 from = other.gameObject.transform.position;
                        from += other.transform.TransformDirection(vectors[i] / 2f * dest);
                        Vector3 to = display.transform.position;
                        to += vectors[j] / 2f * dest;
                        float dist = (from - to).magnitude;
                        //print($"{from}, {to}, {dist}");
                        if (dist < min_dest && ship.CanBePlaced((other.transform.position + vectors[i] * dest - offset) / dest))
                        {
                            where_from = j;
                            where_to = i;
                            from_z = from;
                            to_z = to;
                            nd = other;
                            min_dest = dist;
                        }
                    }
                }
            }
        }
        return (nd, where_from, where_to, from_z, to_z);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startedAt = eventData.position;
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000f);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.GetComponent<ShipBlockDisplay>())
            {
                var display = hit.transform.gameObject.GetComponent<ShipBlockDisplay>();
                if (DeleteMode)
                {
                    if (display.constructed)
                    {
                        if (display.block.type != BlockType.Main)
                        {
                            foreach (var block in ship.DeleteBlock(display.block.number))
                            {
                                Save.instance.session.inventory.Add(block.ToItem());
                            }
                            UpdateShip();
                            UpdateItems();
                        }
                    } 
                    else
                    {
                        Save.instance.session.inventory.Add(nowDisplay.block.type.ToItem());
                        UpdateItems();
                        Destroy(nowDisplay.gameObject);
                        freeBlock = false;
                        nowDisplay = null;
                    }
                }
            }
        }
        DeleteMode = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        particle.transform.position = new Vector3(25, 0, 0);
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        Debug.DrawRay(ray.origin, ray.direction, Color.red);
        RaycastHit[] hits = Physics.RaycastAll(ray, 1000f);
        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.GetComponent<ShipBlockDisplay>())
            {
                var display = hit.transform.gameObject.GetComponent<ShipBlockDisplay>();
                if (display.constructed)
                {
                }
                else
                {
                    if ((clayInfo.Item4 - clayInfo.Item5).magnitude <= range)
                    {
                        //display.transform.Translate(to - from);
                        ShipBlock parent = clayInfo.Item1.block;
                        Vector3 forw = new Vector3(0, 0, -1), back = new Vector3(0, 0, 1), right = new Vector3(-1, 0, 0), 
                            left = new Vector3(1, 0, 0), up = new Vector3(0, 1, 0), down = new Vector3(0, -1, 0);
                        Vector3[] vectors = new Vector3[] { forw, back, up, down, right, left };
                        Vector3 direction = vectors[clayInfo.Item3];
                        ship.AddBlock(display.block.type, clayInfo.Item1.transform.position + direction * dest, 
                            offset, Vector3.zero, dest, parent.number);
                        UpdateShip();
                        freeBlock = false;
                    }
                }
            }
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
                ItemType.AutoFire, ItemType.AutoIce, ItemType.AutoRadiation, ItemType.Engine,
                ItemType.GunModule, ItemType.Wing}.Contains(item))
            {
                GameObject itemBtn = Instantiate(scrollPat, scrollParent);
                itemBtn.GetComponentInChildren<Text>().text = item.ItemToString();
                Button btn = itemBtn.GetComponent<Button>();
                btn.onClick.AddListener(() =>
                {
                    if (!freeBlock)
                    {
                        DeleteMode = false;
                        CreateDetailInWorld(item.ToBlock());
                    }
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
        UpdateContinueButton();
    }
    public void UpdateContinueButton()
    {
        cont.interactable = ship.Engines >= 2;
    }
    public void CreateDetailInWorld(BlockType blockType)
    {
        freeBlock = true;
        ShipBlockDisplay display = Instantiate(blockPrefab).GetComponent<ShipBlockDisplay>();
        nowDisplay = display;
        display.dest = 0.25f;
        display.constructed = false;
        display.block = new ShipBlock(blockType, new Vector3(0, 1, 0), Vector3.zero, Vector3.zero, dest, -1, -1);
        display.offset = offset;
        Save.instance.session.inventory.Remove(blockType.ToItem());
        UpdateItems();
    }

    [ContextMenu("Keep")]
    public void Keep()
    {
        Save.Keep();
    }
    public void NextSpace()
    {
        Save.instance.session.ship = ship;
        Save.Keep();
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
