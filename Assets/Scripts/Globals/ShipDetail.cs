using UnityEngine;
[System.Serializable]
public class ShipDetail
{
    [Newtonsoft.Json.JsonIgnore]
    public Vector3 Position {
        get => new Vector3(pos_x, pos_y, pos_z);
        set { 
            pos_x = value.x;
            pos_y = value.y;
            pos_z = value.z;
        }
    }
    [Newtonsoft.Json.JsonIgnore]
    public Vector3 Rotation
    {
        get => new Vector3(q_x, q_y, q_z);
        set
        {
            q_x = value.x;
            q_y = value.y;
            q_z = value.z;
        }
    }

    public float pos_x, pos_y, pos_z, q_x, q_y, q_z;
    public ItemType item;
    public bool auto = false;
    public ShipDetail(Vector3 position, Vector3 quaternion, ItemType item)
    {
        this.Position = position;
        this.Rotation = quaternion;
        this.item = item;
    }
}

