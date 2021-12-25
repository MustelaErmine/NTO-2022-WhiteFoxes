using System.Collections.Generic;
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

    public static Vector2 WorldToCanvasPostion(Vector3 position)
    {
        Vector2 newPos = Camera.main.WorldToScreenPoint(position);
        newPos = new Vector2(newPos.x / Screen.width * 1280, newPos.y / Screen.height * 720);
        return newPos;
    }

    public static string ItemToString(this ItemType item)
    {
        switch (item)
        {
            case ItemType.Food:
                return "Еда";
            case ItemType.Water:
                return "Вода";
            case ItemType.SpecialDetail:
                return "Спец. деталь";
            case ItemType.Bonus:
                return "Бонус";
            case ItemType.Case:
                return "Кейс";
            case ItemType.DetailBook:
                return "Записка";
            case ItemType.DetailFire:
                return "Огненная пушка";
            case ItemType.DetailIce:
                return "Электропушка";
            case ItemType.DetailRadiation:
                return "Радиационная пушка";
            case ItemType.Drawing:
                return "Чертеж";
            case ItemType.Fuel:
                return "Топливо";
            case ItemType.Skin:
                return "Скин";
            default:
                return "Error";
        }
    }

    public static void Shuffle(this List<int> list)
    {
        List<(int, int)> rand = new List<(int, int)>(list.Count);
        for (int i = 0; i < list.Count; i ++)
        {
            rand.Add((ProceduralGeneration.instance.Next(), list[i]));
        }
        rand.Sort();
        for (int i = 0; i < list.Count; i++)
        {
            list[i] = rand[i].Item2;
        }
    }
}
