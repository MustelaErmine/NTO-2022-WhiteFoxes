using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetControl : MonoBehaviour
{
    public void Generate()
    {
        _generationNumber = ProceduralGeneration.instance.Next();
        type = _generationNumber % 4;

        GetComponentInChildren<MeshFilter>().mesh = planetTypes[type];
        GetComponentInChildren<MeshRenderer>().material.mainTexture = planetTexts[type];
        if (type > 2)
        {
            transform.GetChild(0).localScale = new Vector3(7.5f, 7.5f, 7.5f);
        }
        else
        {
            transform.GetChild(0).localScale = new Vector3(7.5f, 7.5f, 7.5f);
        }
        float delta = _generationNumber % 2003 + 2000;
        Vector3 direction = new Vector3(_generationNumber % 137, _generationNumber % 139, _generationNumber % 149);
        direction = direction.normalized;

        transform.Translate(direction * delta);

        itemsGenerations = new int[4] { 0, 0, 0, 0 };
        items = new List<Item>();
        List<Item> displayedItems = new List<Item>();
        for (int i = 0; i < 4; i++)
        {
            itemsGenerations[i] = ProceduralGeneration.instance.Next();
            Item item = GetNumberedItem(itemsGenerations[i], 5);
            items.Add(item);
            if (i < Save.instance.session.skills[Skills.Monitor])
                displayedItems.Add(items[i]);
        }

        myPanel.GetChild(1).GetComponent<Text>().text = Item.ListToString(displayedItems.ToArray());
    }
    int _generationNumber = 0;
    int type = 0;

    public RectTransform myPanel;
    new Rigidbody rigidbody;
    public int[] itemsGenerations = new int[4];
    public static int useType;
    List<Item> items;
    [SerializeField] Mesh[] planetTypes;
    [SerializeField] Texture[] planetTexts;
     
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
        Landing();
    }

    void Landing()
    {
        SpaceControl.ShowMessageStatic("Вы высаживаетесь на эту планету", () =>
        {
            for (int i = 0; i < 4; i++)
            {
                Item item = items[i];
                if (item == null)
                {
                    continue;
                }
                if (item.type == ItemType.None)
                {
                    continue;
                }
                if (item.type == ItemType.Case)
                {
                    Save.instance.session.cases.Add(item);
                }
                else
                {
                    Save.instance.session.inventory.Add(item.type);
                }
            }
            useType = type;
            UnityEngine.SceneManagement.SceneManager.LoadScene("OnPlanet");
        });
    }

    public static Item GetNumberedItem(int num, int typeMod)
    {
        Item item = new Item();
        switch (num % typeMod)
        {
            case 0:
                item.type = ItemType.None;
                break;
            case 1:
                item.type = ItemType.Case;
                break;
            case 2:
                //if (num % 6 == 0)
                //    item.type = ItemType.DetailFire;
                //else if (num % 6 == 1)
                //    item.type = ItemType.DetailIce;
                //else if (num % 6 == 2)
                //    item.type = ItemType.DetailRadiation;
                //else if (num % 6 == 3)
                //    item.type = ItemType.AutoFire;
                //else if (num % 6 == 4)
                //    item.type = ItemType.AutoIce;
                //else if (num % 6 == 5)
                //    item.type = ItemType.AutoRadiation;
                item.type = ItemType.Water;
                break;
            case 3:
                item.type = ItemType.SpecialDetail;
                break;
            case 4:
                //if (num % 2 == 0)
                //    item.type = ItemType.Water;
                //else
                //    item.type = ItemType.Food;
                item.type = ItemType.Food;
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
                int caseItem = num % 8;
                if (caseItem == 0)
                    item.caseItem = ItemType.Bonus;
                else if (caseItem == 1)
                    item.caseItem = ItemType.Drawing;
                else if (caseItem == 2)
                    item.caseItem = ItemType.SpecialDetail;
                else if (caseItem == 3)
                    item.caseItem = ItemType.AutoFire;
                else if (caseItem == 4)
                    item.caseItem = ItemType.AutoIce;
                else if (caseItem == 5)
                    item.caseItem = ItemType.AutoRadiation;
                else
                    item.caseItem = ItemType.Skin;
            }
        }
        return item;
    }
}
