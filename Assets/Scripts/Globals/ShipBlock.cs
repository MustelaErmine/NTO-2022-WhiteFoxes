using UnityEngine;
using System;

[Serializable]
public struct ShipBlock
{
    public int x, y, z;
    [Newtonsoft.Json.JsonIgnore]
    public Vector3 Position { get => new Vector3(x, y, z); }
    public float rot_x, rot_y, rot_z;
    [Newtonsoft.Json.JsonIgnore]
    public Vector3 Rotation { get => new Vector3(rot_x, rot_y, rot_z); }
    public int number, parent;
    public BlockType type;

    public ShipBlock(BlockType type, Vector3 position, Vector3 offset, Vector3 rotation, float dest, int number, int parent=-1)
    {
        position -= offset;
        position /= dest;
        Vector3Int vec = Vector3Int.RoundToInt(position);
        x = vec.x;
        y = vec.y;
        z = vec.z;
        this.type = type;
        this.number = number;
        this.parent = parent;
        vec = Vector3Int.RoundToInt(rotation);
        rot_x = vec.x;
        rot_y = vec.y;
        rot_z = vec.z;
    }
    public new string ToString()
    {
        return $"{type} in ({x}, {y}, {z}), number {number} from parent {parent}";
    }
}
