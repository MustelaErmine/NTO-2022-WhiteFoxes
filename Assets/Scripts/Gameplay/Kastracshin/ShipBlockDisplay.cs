using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBlockDisplay : MonoBehaviour
{
    public ShipBlock block;
    public Vector3 offset;
    public float dest;
    void Start()
    {
        SetUpBlock();
    }
    public void SetUpBlock()
    {
        Mesh mesh;
        Texture texture;
        (Mesh, Texture) pair = Ship.GetBlockDisplay(block.type);
        mesh = pair.Item1;
        texture = pair.Item2;
        GetComponent<MeshRenderer>().material.mainTexture = texture;
        GetComponent<MeshFilter>().mesh = mesh;
        transform.position = offset + block.Position * dest;
        transform.eulerAngles = block.Rotation;
    }
}
