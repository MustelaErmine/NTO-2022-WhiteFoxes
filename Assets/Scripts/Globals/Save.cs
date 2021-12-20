using UnityEngine;
using System.IO;
using Newtonsoft.Json;

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
    public Session session;
    public int record;

    public Save()
    {
        session = null;
        record = 0;
    }
    public Save(string name) : this()
    {
        playerName = name;
    }

    public static void Load()
    {
        if (_instance != null)
            return;

        if (!File.Exists(path))
        {
            instance = new Save();
            Keep();
        }
        instance = JsonConvert.DeserializeObject<Save>(File.ReadAllText(path));
        if (_instance.session != null)
        {
            ProceduralGeneration.instance = new ProceduralGeneration(_instance.session.randomSeed,
                                                                     _instance.session.randomGenerationsWasInOldStep);
        }
    }
    public static void Keep()
    {
        File.WriteAllText(path, JsonConvert.SerializeObject(_instance, new JsonSerializerSettings{}));
    }
    public static void Die()
    {
        instance.record = Mathf.Max(instance.record, instance.session.years);
        instance.session = null;
        Save.Keep();
    }
}
