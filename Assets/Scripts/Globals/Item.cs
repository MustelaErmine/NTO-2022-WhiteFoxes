using System.Collections.Generic;

public class Item
{
    public ItemType type;
    public CaseType caseType;
    public ItemType caseItem;

    public override string ToString()
    {
        if (type == ItemType.Case)
            return type.ItemToString() + " " + caseType.ToString();
        return type.ItemToString();
    }
    public static string ListToString(Item[] items)
    {
        Dictionary<string, int> pairs = new Dictionary<string, int>();
        foreach (Item item in items)
        {
            string name = item.ToString();
            if (!pairs.ContainsKey(name))
            {
                pairs[name] = 0;
            }
            pairs[name] += 1;
        }
        string output = "";
        foreach (string key in pairs.Keys)
        {
            output += key;
            if (pairs[key] > 1)
            {
                output += " x" + pairs[key].ToString();
            }
            output += "\n";
        }
        return output;
    }
}