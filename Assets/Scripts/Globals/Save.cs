using UnityEngine;
using System.IO;

public class Save
{
    private static Save _instance;
    private static readonly string path = Application.persistentDataPath + @"\save.json";

    public static Save instance
    {
        get
        {
            if (_instance == null)
                Load();
            return _instance;
        }
        set => _instance = value;
    }

    public bool firstEnter = true;
    public string playerName = "AaronEnjoyer";
    public long randomSeed = 37613761;

    public Save()
    {
        randomSeed = ((System.DateTimeOffset)System.DateTime.Now).ToUnixTimeSeconds();
    }
    public Save(string name) : this()
    {
        playerName = name;
    }

    public static void Load()
    {
        if (instance != null)
            return;

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
