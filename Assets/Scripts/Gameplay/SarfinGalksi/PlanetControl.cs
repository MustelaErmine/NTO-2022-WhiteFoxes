using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetControl : MonoBehaviour
{
    public void Generate()
    {
        _generationNumber = ProceduralGeneration.instance.Next();
        type = _generationNumber % 3;

        //Temp
        GetComponentInChildren<MeshFilter>().mesh = planetTypes[type];
        //EndTemp
        float delta = _generationNumber % 2003 + 2000;
        Vector3 direction = new Vector3(_generationNumber % 137, _generationNumber % 139, _generationNumber % 149);
        direction = direction.normalized;

        transform.Translate(direction * delta);

        int challengeGen = _generationNumber % 101;
        if (challengeGen < 4)
            challenge = 0;
        else if (challengeGen < 52)
            challenge = 1;
        else
            challenge = 2;

        string itemsText = "";
        itemsGenerations = new int[4] { 0, 0, 0, 0 };
        items = new List<Item>();
        for (int i = 0; i < Mathf.Min(4, Save.instance.session.skills[Skills.Monitor]); i++)
        {
            itemsGenerations[i] = ProceduralGeneration.instance.Next();
            items.Add(GetNumberedItem(itemsGenerations[i], 13));
            itemsText += GetNameOfItem(items[i]) + "\n";
        }

        myPanel.GetChild(1).GetComponent<Text>().text += itemsText;
    }
    int _generationNumber = 0;
    int type = 0;
    int challenge = 0;

    public RectTransform myPanel;
    new Rigidbody rigidbody;
    public int[] itemsGenerations = new int[4];
    List<Item> items;
    [SerializeField] Mesh[] planetTypes;
     
    new Transform transform
    {
        get
        {
            if (_transform == null)
                _transform = GetComponent<Transform>();
            return _transform;
        }
    }
    Transform _transform;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    public void OnTriggerEnter(Collider other)
    {
        string chal = "";
        switch (challenge)
        {
            case 0:
                chal = "Все хорошо, вашей безопасности ничего не угрожает.";
                break;
            case 1:
                chal = "На планете природные катоклизмы. Вам нужны: 2 еды, 1 вода и 1 топливо";
                break;
            case 2:
                chal = "На планете на вас напали. Вам нужен корабль мощностью 5.";
                break;
        }
        if (other.tag == "Player")
            SpaceControl.ShowMessageStatic(chal, Landing);
    }

    void Landing()
    {
        print("Landing");
        switch (challenge)
        {
            case 0:
                break;
            case 1:
                foreach (ItemType item in new ItemType[] { ItemType.Food, ItemType.Fuel, ItemType.Water, ItemType.Food })
                    if (!Save.instance.session.inventory.Contains(item))
                    {
                        SpaceControl.Die();
                        return;
                    }
                    else
                        Save.instance.session.inventory.Remove(item);
                break;
            case 2:
                //ToDo: check speisheep power
                break;
        }
        SpaceControl.ShowMessageStatic("Все хорошо, вы выжили и можете отправляться в следующее приключение.", () =>
        {
            foreach (Item item in items)
            {
                if (item.type == ItemType.Case)
                {
                    print(Save.instance);
                    print(Save.instance.session);
                    print(Save.instance.session.inventory);
                    print(item);
                    Save.instance.session.inventory.Add(item.caseItem);
                }
                else
                {
                    Save.instance.session.inventory.Add(item.type);
                }
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene("OnPlanet");
        });
    }

    public static Item GetNumberedItem(int num, int typeMod)
    {
        Item item = new Item();
        switch (num % typeMod)
        {
            case 0:
                item.type = ItemType.DetailBook;
                break;
            case 1:
                item.type = ItemType.Case;
                break;
            case 2:
                item.type = ItemType.DetailFire;
                break;
            case 3:
                item.type = ItemType.DetailIce;
                break;
            case 4:
                item.type = ItemType.DetailRadiation;
                break;
            default:
                if ((num - 4) % 3 == 0)
                    item.type = ItemType.Food;
                else if ((num - 4) % 3 == 1)
                    item.type = ItemType.Water;
                else
                    item.type = ItemType.Fuel;
                break;
        }
        int caset = num % 100003;
        if (item.type == ItemType.Case)
        {
            if (caset <= 1)
                item.caseType = CaseType.Black;
            else if (caset <= 91)
                item.caseType = CaseType.Legendary;
            else if (caset <= 300)
                item.caseType = CaseType.Epic;
            else if (caset <= 17000)
                item.caseType = CaseType.Rare;
            else if (caset <= 35000)
                item.caseType = CaseType.Unsimple;
            else if (caset <= 60000)
                item.caseType = CaseType.Simple;
            else
                item.caseType = CaseType.Opened;
            if (new List<CaseType> { CaseType.Opened, CaseType.Simple }.Contains(item.caseType))
            {
                int caseItem = num % 3;
                if (caseItem == 0)
                    item.caseItem = ItemType.Skin;
                else if (caseItem == 1)
                    item.caseItem = ItemType.Drawing;
                else
                    item.caseItem = ItemType.SpecialDetail;
            }
            else
            {
                int caseItem = num % 5;
                if (caseItem == 0)
                    item.caseItem = ItemType.Bonus;
                else if (caseItem == 1)
                    item.caseItem = ItemType.Drawing;
                else if (caseItem == 2)
                    item.caseItem = ItemType.SpecialDetail;
                else
                    item.caseItem = ItemType.Skin;
            }
        }
        return item;
    }
    public static string GetNameOfItem(Item item)
    {
        if (item.type == ItemType.Case)
            return item.type.ToString() + " " + item.caseType.ToString();
        return item.type.ToString();
    }
}
