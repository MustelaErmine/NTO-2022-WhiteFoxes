using UnityEngine;

public static class Utils
{
    public static Quaternion Add(this Quaternion q1, Quaternion q2)
    {
        return new Quaternion(q1.x + q2.x, q1.y + q2.y, q1.z + q2.z, q1.w + q2.w);
    }

    public static Quaternion Sub(this Quaternion q1, Quaternion q2)
    {
        return new Quaternion(q1.x - q2.x, q1.y - q2.y, q1.z - q2.z, q1.w - q2.w);
    }
}
