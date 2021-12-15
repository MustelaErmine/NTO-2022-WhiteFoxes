using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Save
{
    public static Save instance;
    private static readonly string path = Application.persistentDataPath + @"\save.json";

    public bool firstEnter = true;
    public string playerName = "AaronEnjoyer";

    public static void Load()
    {
        if (!File.Exists(path))
        {
            instance = new Save();
            Keep();
        }
        instance = JsonUtility.FromJson<Save>(File.ReadAllText(path));
    }
    public static void Keep()
    {
        File.WriteAllText(path, JsonUtility.ToJson(instance));
    }
}
