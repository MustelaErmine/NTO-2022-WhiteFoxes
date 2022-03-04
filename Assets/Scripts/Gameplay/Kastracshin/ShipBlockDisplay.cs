using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBlockDisplay : MonoBehaviour
{
    public ShipBlock block;
    public Vector3 offset;
    public float dest;
    public bool constructed = false;

    public Transform forw, back, up, down, right, left;
    void Start()
    {
        SetUpBlock();
    }
    public void SetUpBlock()
    {
        Mesh mesh;
        Material texture;
        (Mesh, Material) pair = Ship.GetBlockDisplay(block.type);
        mesh = pair.Item1;
        texture = pair.Item2;
        GetComponentInChildren<MeshRenderer>().material = texture;
        GetComponentInChildren<MeshFilter>().mesh = mesh;
        transform.position = offset + block.Position * dest;
        transform.eulerAngles = block.Rotation;
        if (block.number == 0 || !constructed)
        {
            transform.localScale = Vector3.one * dest;
        }
        if (block.type == BlockType.Engine || block.type == BlockType.GunModule)
        {
            transform.GetChild(0).localPosition = new Vector3(-0.34f, -0.435f, 0);
            transform.GetChild(0).localScale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        else
        {
            transform.GetChild(0).localPosition = Vector3.zero;
            transform.GetChild(0).localScale = Vector3.one;
        }
        if (block.type == BlockType.Engine)
        {
            transform.GetChild(0).eulerAngles = new Vector3(0, 90, 0);
            transform.GetChild(0).localPosition = new Vector3(0f, -0.5f, 0);
        }
        if (new List<BlockType> { BlockType.AutoFire, BlockType.AutoIce, BlockType.AutoRadiation,
            BlockType.DetailFire, BlockType.DetailIce, BlockType.DetailRadiation}.Contains(block.type))
        {
            transform.GetChild(0).eulerAngles = new Vector3(0, 90, 0);
            transform.GetChild(0).localScale = new Vector3(.25f, .25f, .25f);
            transform.GetChild(0).localPosition = new Vector3(0, -0.43f, -0.25f);
        }
        if (block.type == BlockType.LeftWing)
        {
            transform.GetChild(0).localPosition = new Vector3(-0.126f, -0.439f, 0);
            transform.GetChild(0).localScale = new Vector3(0.25f, 0.25f, 0.25f);
            transform.GetChild(0).eulerAngles = new Vector3(0, 90, 0);
        }
        if (block.type == BlockType.Main)
        {
            transform.GetChild(0).localPosition = new Vector3(0.013f, -0.439f, -0.018f);
            transform.GetChild(0).localScale = new Vector3(0.25f, 0.25f, 0.25f);
            transform.GetChild(0).eulerAngles = new Vector3(0, 90, 0);
        }
        if (new List<BlockType> { BlockType.AutoFire, BlockType.AutoIce, BlockType.AutoRadiation,
            BlockType.DetailFire, BlockType.DetailIce, BlockType.DetailRadiation}.Contains(block.type))
        {
            forw.gameObject.SetActive(false);
            back.gameObject.SetActive(false);
            right.gameObject.SetActive(false);
            left.gameObject.SetActive(false);
            up.gameObject.SetActive(false);
            down.gameObject.SetActive(true);
        }
        if (block.type == BlockType.Main)
        {
            forw.gameObject.SetActive(false);
            back.gameObject.SetActive(true);
            right.gameObject.SetActive(true);
            left.gameObject.SetActive(true);
            up.gameObject.SetActive(false);
            down.gameObject.SetActive(false);
        }
        if (block.type == BlockType.GunModule)
        {
            forw.gameObject.SetActive(true);
            back.gameObject.SetActive(true);
            right.gameObject.SetActive(true);
            left.gameObject.SetActive(true);
            up.gameObject.SetActive(true);
            down.gameObject.SetActive(true);
        }
        if (block.type == BlockType.Engine)
        {
            forw.gameObject.SetActive(true);
            back.gameObject.SetActive(false);
            right.gameObject.SetActive(true);
            left.gameObject.SetActive(true);
            up.gameObject.SetActive(false);
            down.gameObject.SetActive(false);
        }
        if (block.type == BlockType.LeftWing)
        {
            forw.gameObject.SetActive(false);
            back.gameObject.SetActive(false);
            right.gameObject.SetActive(true);
            left.gameObject.SetActive(false);
            up.gameObject.SetActive(false);
            down.gameObject.SetActive(false);
        }
        if (block.type == BlockType.Wing)
        {
            forw.gameObject.SetActive(false);
            back.gameObject.SetActive(false);
            right.gameObject.SetActive(false);
            left.gameObject.SetActive(true);
            up.gameObject.SetActive(false);
            down.gameObject.SetActive(false);
        }
        if (block.type == BlockType.Main)
        {
            forw.gameObject.SetActive(false);
            back.gameObject.SetActive(true);
            right.gameObject.SetActive(false);
            left.gameObject.SetActive(false);
            up.gameObject.SetActive(false);
            down.gameObject.SetActive(false);
        }
    }
}
