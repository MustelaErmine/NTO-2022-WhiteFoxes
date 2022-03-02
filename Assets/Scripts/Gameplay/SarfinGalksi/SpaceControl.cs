using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceControl : MonoBehaviour
{
    public PlanetControl[] planets;
    public Slider energySlider, fuelSlider;
    public ShipMoving ship;
    public int randomEventGeneration;

    public RectTransform[] planetsPanels;
    public static SpaceControl instance;
    public RectTransform messageBox;
    [SerializeField] Text food, water, years, invent;
    [SerializeField] AudioClip notif;

    public float energy = 0;
    public float hyperFuel = 0;
    public bool highPressed = false;

    Action answeredAction;

    void Awake()
    {
        instance = this;
        Save.Load();
        if (Save.instance.session == null)
        {
            Save.instance.session = new Session();
            Save.instance.session.inventory.Add(ItemType.Water);
            Save.instance.session.inventory.Add(ItemType.Food);
            Save.instance.session.inventory.Add(ItemType.Fuel);
        }
        Save.instance.session.step += 1;
        Save.Keep();
        foreach (RectTransform rectTransform in planetsPanels)
        {
            rectTransform.gameObject.SetActive(false);
        }
        for (byte i = 0; i < 3; i++)
        {
            planets[i].myPanel = planetsPanels[i];
            planets[i].Generate();
        }

        Application.targetFrameRate = 60;

        energy = 1;
        hyperFuel = 1;

        food.text = Save.instance.session.inventory.FindAll((ItemType t) => t == ItemType.Food).Count.ToString();
        water.text = Save.instance.session.inventory.FindAll((ItemType t) => t == ItemType.Water).Count.ToString();
        years.text = Save.instance.session.years.ToString();
        List<Item> inv = new List<Item>();
        foreach(ItemType item in Save.instance.session.inventory)
        {
            if (item != ItemType.Food && item != ItemType.Water)
                inv.Add(new Item { type = item, caseItem = ItemType.None, caseType = CaseType.Unsimple });
        }
        invent.text = Item.ListToString(inv.ToArray());
    }
    private void Start()
    {
        randomEventGeneration = ProceduralGeneration.instance.Next();

        if (randomEventGeneration % 101 < 45)
        {
            CreateRandomEvent();
        }
#if !UNITY_EDITOR
        try
        {
            for (int i = 0; i < Mathf.CeilToInt((float)Save.instance.session.step / 2f); i++)
            {
                if (!Save.instance.session.inventory.Remove(ItemType.Water))
                    throw new Exception();
            }
            for (int i = 0; i < Mathf.CeilToInt((float)Save.instance.session.step / 2f); i++)
            {
                if (!Save.instance.session.inventory.Remove(ItemType.Food))
                    throw new Exception();
            }
        }
        catch
        {
            Die("У вас недостаточно ресурсов для продолжения приключения");
        }
#endif
        food.text = Save.instance.session.inventory.FindAll((ItemType t) => t == ItemType.Food).Count.ToString();
        water.text = Save.instance.session.inventory.FindAll((ItemType t) => t == ItemType.Water).Count.ToString();
        years.text = Save.instance.session.years.ToString();
    }

    void Update()
    {
        float newTimeScale = 1f;
        /*
        if (Input.GetKey(KeyCode.LeftControl) && energy > 0f)
        {
            energy -= 1f / Save.instance.session.skills[Skills.TimeSlowCapacity] * Time.unscaledDeltaTime;
            newTimeScale *= 0.5f;
        } 
        
        else */if (highPressed && energy > 0f)
        {
            energy -= 1f / Save.instance.session.skills[Skills.TimeSpeedCapacity] * Time.unscaledDeltaTime;
            newTimeScale *= 2f;
        } 
        else
        {
            energy += 1f / 40f * Time.unscaledDeltaTime;
        }

        if (Time.timeScale != newTimeScale)
        {
            Time.timeScale = newTimeScale;
        }

        energy = Mathf.Min(energy, 1f);
        energy = Mathf.Max(0f, energy);

        hyperFuel += 1f / Save.instance.session.skills[Skills.HyperDriveRecharge] * Time.unscaledDeltaTime;
        hyperFuel = Mathf.Min(hyperFuel, 1f);

        energySlider.value = energy;
        fuelSlider.value = hyperFuel;

        for (int i = 0; i < 3; i++)
        {
            planetsPanels[i].anchoredPosition = Utills.WorldToCanvasPostion(planets[i].transform.position) +
                new Vector2(Screen.width / 20f, 0);
            if (!planets[i].GetComponentInChildren<MeshRenderer>().isVisible)
            {
                planetsPanels[i].gameObject.SetActive(false);
            }
        }
    }
    public void HyperJump()
    {
        if (Mathf.Abs(hyperFuel - 1f) < 1e-5)
        {
            ship.transform.Translate(ship.transform.forward * Save.instance.session.skills[Skills.HyperDriveForce] * 1000f);
            hyperFuel = 0;
        }
    }
    public void PlanetInfo()
    {
        for (int i = 0; i < 3; i++)
        {
            if (Mathf.Abs((ship.transform.position - planets[i].transform.position).magnitude) < 2000f)
            {
                planetsPanels[i].gameObject.SetActive(true);
            }
        }
    }

    public void CreateRandomEvent()
    {
        string where = "";
        switch (randomEventGeneration % 3) {
            case 0:
                where = "Заброшенная планета";
                break;
            case 1:
                where = "Заправка";
                break;
            case 2:
                where = "Космическая станция империи Авион";
                break;
        }
        where += "\n";
        string founded = "Вы там нашли: ";
        Item item = PlanetControl.GetNumberedItem(randomEventGeneration, 5);
        if (item.type == ItemType.Case)
        {
            founded += "кейс.";
        }
        else if (item.type == ItemType.DetailBook)
        {
            founded += "таинственную записку.";
        }
        else
        {
            founded += "деталь.";
        }
        founded += "\n";
        ShowMessageStatic("Внимание!\n" + where + founded);
    }

    public void ShowMessage(string text, Action action)
    {
        messageBox.gameObject.SetActive(true);
        messageBox.GetChild(1).GetComponent<Text>().text = text;
        messageBox.GetComponent<AudioSource>().PlayOneShot(notif);
        ship.enabled = false;
        ship.rigidbody.velocity = Vector3.zero;
        answeredAction = action;
        //StartCoroutine(WaitToButton(action));
    }
    public static void ShowMessageStatic(string text, Action action = null)
    {
        instance.ShowMessage(text, action);
    }
    public void StopWaiting()
    {
        StartCoroutine(StopWaitingCoroutine());
    }
    public IEnumerator StopWaitingCoroutine()
    {
        messageBox.gameObject.SetActive(false);
        ship.enabled = true;
        yield return new WaitForSeconds(0.01f);
        answeredAction?.Invoke();
    }
    public IEnumerator WaitToButton(Action action)
    {
        while (!Input.GetKeyDown(KeyCode.C))
            yield return null;
        messageBox.gameObject.SetActive(false);
        ship.enabled = true;
        yield return new WaitForSeconds(0.01f);
        action?.Invoke();
    }
    public static void Die(string text= "Вы умерли от недостатка ресурсов")
    {
        ShowMessageStatic(text, () =>
        {
            Save.Die();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        });
    }
}
