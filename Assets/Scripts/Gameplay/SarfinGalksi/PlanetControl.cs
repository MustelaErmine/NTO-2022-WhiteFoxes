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
        Color color = Color.black;

        switch (type)
        {
            case 0:
                color = Color.red;
                break;
            case 1:
                color = Color.blue;
                break;
            case 2:
                color = Color.green;
                break;
            case 3:
                color = Color.cyan;
                break;
        }
        //Temp
        GetComponentInChildren<MeshRenderer>().material.color = color;
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
        itemsGenerations = new int[4] {0, 0, 0, 0};
        items = new List<Item>();
        for (int i = 0; i < Mathf.Min(4, Save.instance.session.skills[Skills.Monitor]); i++)
        {
            itemsGenerations[i] = ProceduralGeneration.instance.Next();
            items.Add(GetNumberedItem(itemsGenerations[i]));
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
        if (other.tag == "Player")
            Landing();
    }

    void Landing()
    {
        SpaceControl.ShowMessageStatic("You have challenge " + challenge.ToString());
        switch (challenge)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
        }
    }

    public Item GetNumberedItem(int num)
    {
        Item item = new Item();
        switch (num % 13)
        {
            case 0:
                item.type = InventoryItemType.DetailBook;
                break;
            case 1:
                item.type = InventoryItemType.Case;
                break;
            case 2:
                item.type = InventoryItemType.DetailFire;
                break;
            case 3:
                item.type = InventoryItemType.DetailIce;
                break;
            case 4:
                item.type = InventoryItemType.DetailRadiation;
                break;
            default:
                if ((num - 4) % 3 == 0)
                    item.type = InventoryItemType.Food;
                else if ((num - 4) % 3 == 1)
                    item.type = InventoryItemType.Water;
                else
                    item.type = InventoryItemType.Fuel;
                break;
        }
        int caset = num % 100003;
        if (item.type == InventoryItemType.Case)
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
                    item.caseItem = InventoryItemType.Skin;
                else if (caseItem == 1)
                    item.caseItem = InventoryItemType.Drawing;
                else
                    item.caseItem = InventoryItemType.SpecialDetail;
            }
            else
            {
                int caseItem = num % 5;
                if (caseItem == 0)
                    item.caseItem = InventoryItemType.Bonus;
                else if (caseItem == 1)
                    item.caseItem = InventoryItemType.Drawing;
                else if (caseItem == 2)
                    item.caseItem = InventoryItemType.SpecialDetail;
                else
                    item.caseItem = InventoryItemType.Skin;
            }
        }
        return item;
    }
    public string GetNameOfItem(Item item)
    {
        if (item.type == InventoryItemType.Case)
            return item.type.ToString() + " " + item.caseType.ToString();
        return item.type.ToString();
    }
}
