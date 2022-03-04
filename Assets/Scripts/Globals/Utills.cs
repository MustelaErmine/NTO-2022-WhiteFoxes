using System;
using System.Collections.Generic;
using UnityEngine;

public static class Utills
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
            case ItemType.AutoFire:
                return "Авто-Огненная пушка";
            case ItemType.AutoIce:
                return "Авто-Электропушка";
            case ItemType.AutoRadiation:
                return "Авто-Радиационная пушка";
            case ItemType.GunModule:
                return "Оруж. модуль";
            case ItemType.Engine:
                return "Двигатель";
            case ItemType.Wing:
                return "Крыло";
            case ItemType.WingLeft:
                return "Лев. крыло";
            default:
                return item.ToString();
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
    public static BlockType ToBlock(this ItemType item)
    {
        switch (item)
        {
            case ItemType.AutoFire:
                return BlockType.AutoFire;
            case ItemType.AutoIce:
                return BlockType.AutoIce;
            case ItemType.AutoRadiation:
                return BlockType.AutoRadiation;
            case ItemType.DetailFire:
                return BlockType.DetailFire;
            case ItemType.DetailIce:
                return BlockType.DetailIce;
            case ItemType.DetailRadiation:
                return BlockType.DetailRadiation;
            case ItemType.Wing:
                return BlockType.Wing;
            case ItemType.GunModule:
                return BlockType.GunModule;
            case ItemType.Engine:
                return BlockType.Engine;
            case ItemType.WingLeft:
                return BlockType.LeftWing;
            default:
                return BlockType.Wing;
        }
    }
    public static ItemType ToItem(this BlockType block)
    {
        switch (block)
        {
            case BlockType.Engine:
                return ItemType.Engine;
            case BlockType.Wing:
                return ItemType.Wing;
            case BlockType.GunModule:
                return ItemType.GunModule;
            case BlockType.AutoFire:
                return ItemType.AutoFire;
            case BlockType.AutoIce:
                return ItemType.AutoIce;
            case BlockType.AutoRadiation:
                return ItemType.AutoRadiation;
            case BlockType.DetailFire:
                return ItemType.DetailFire;
            case BlockType.DetailIce:
                return ItemType.DetailIce;
            case BlockType.DetailRadiation:
                return ItemType.DetailRadiation;
            case BlockType.LeftWing:
                return ItemType.WingLeft;
            default:
                return ItemType.Wing;
        }
    }
}
