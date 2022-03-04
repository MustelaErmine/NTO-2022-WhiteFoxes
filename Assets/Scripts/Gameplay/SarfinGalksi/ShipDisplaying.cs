using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ShipDisplaying : MonoBehaviour
{
    [SerializeField] GameObject detail;
    [SerializeField] GameObject blockPrefab;
    ShipOnPlanet ship;
    public bool work = false;
    public float scale = 1f;
    public Vector3 offset;
    public float rotation;
    public Transform mainShape;
    List<ShipBlockDisplay> bl = new List<ShipBlockDisplay>();
    void Awake()
    {
        Save.Load();
        ShipDetail[] list = new ShipDetail[Save.instance.session.shipDetails.Count];
        Save.instance.session.shipDetails.CopyTo(list);
        List<Detail> details = new List<Detail>();
        foreach (ShipBlock item in Save.instance.session.ship.blocks)
        {
            details.Add(CreateDetail(item));
        }
        ship = GetComponent<ShipOnPlanet>();
        if (ship != null)
        {
            ship.details = details.Where(d => d!=null && new List<ItemType> {ItemType.DetailFire, ItemType.DetailIce,
                ItemType.DetailRadiation}.Contains(d.DetailType)).ToArray();
        }
        mainShape.eulerAngles = new Vector3(0, rotation, 0);
        mainShape.localScale = Vector3.one * scale;
    }
    private void Start()
    {
    }
    private void Update()
    {
    }
    public Detail CreateDetail(ShipBlock block)
    {
        print(block.ToString());
        if (!new List<BlockType> { BlockType.AutoFire, BlockType.AutoIce, BlockType.AutoRadiation ,
            BlockType.DetailFire, BlockType.DetailIce, BlockType.DetailRadiation}.Contains(block.type))
        {
            GameObject display = Instantiate(blockPrefab);
            display.GetComponent<ShipBlockDisplay>().enabled = false;
            Mesh mesh;
            Material texture;
            (Mesh, Material) pair = Ship.GetBlockDisplay(block.type);
            mesh = pair.Item1;
            texture = pair.Item2;
            display.GetComponentInChildren<MeshRenderer>().material = texture;
            display.GetComponentInChildren<MeshFilter>().mesh = mesh;
            display.transform.position = offset + block.Position;
            display.transform.eulerAngles = block.Rotation;
            if (block.number == 0)
            {
                display.transform.localScale = Vector3.one;
            }
            if (block.type == BlockType.Engine || block.type == BlockType.GunModule)
            {
                display.transform.GetChild(0).localPosition = new Vector3(-0.34f, -0.435f, 0);
                display.transform.GetChild(0).localScale = new Vector3(0.25f, 0.25f, 0.25f);
            }
            else
            {
                display.transform.GetChild(0).localPosition = Vector3.zero;
                display.transform.GetChild(0).localScale = Vector3.one;
            }
            if (block.type == BlockType.Engine)
            {
                display.transform.GetChild(0).eulerAngles = new Vector3(0, 90, 0);
                display.transform.GetChild(0).localPosition = new Vector3(0f, -0.5f, 0);
            }
            if (block.type == BlockType.LeftWing)
            {
                display.transform.GetChild(0).localPosition = new Vector3(-0.126f, -0.439f, 0);
                display.transform.GetChild(0).localScale = new Vector3(0.25f, 0.25f, 0.25f);
                display.transform.GetChild(0).eulerAngles = new Vector3(0, 90, 0);
            }
            if (block.type == BlockType.Wing)
            {
                display.transform.GetChild(0).localPosition = new Vector3(-0.126f, -0.439f, 0);
                display.transform.GetChild(0).localScale = new Vector3(0.25f, 0.25f, 0.25f);
                display.transform.GetChild(0).eulerAngles = new Vector3(0, 90, 0);
            }
            if (block.type == BlockType.Main)
            {
                display.transform.GetChild(0).localPosition = new Vector3(-0.126f, -0.439f, 0);
                display.transform.GetChild(0).localScale = new Vector3(0.25f, 0.25f, 0.25f);
                display.transform.GetChild(0).eulerAngles = new Vector3(0, 90, 0);
            }
            display.transform.parent = mainShape;
            return null;
        }
        else
        {
            GameObject obj = Instantiate(detail);
            obj.transform.localPosition = block.Position + offset;
            obj.transform.eulerAngles = new Vector3(-90, 90, 0);
            obj.transform.localPosition -= new Vector3(0, 0.43f, 0);
            obj.transform.localScale = Vector3.one * 2f;
            Detail detailController = obj.GetComponent<Detail>();
            detailController.DetailType = block.type.ToItem();
            detailController.work = work;
            obj.transform.parent = mainShape;
            return detailController;
        }
    }
}
