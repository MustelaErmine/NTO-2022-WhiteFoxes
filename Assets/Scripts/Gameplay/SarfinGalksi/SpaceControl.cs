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

    public float energy = 0;
    public float hyperFuel = 0;

    void Start()
    {
        instance = this;
        if (Save.instance.session == null)
        {
            Save.instance.session = new Session();
        }
        Save.Load();
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

        randomEventGeneration = ProceduralGeneration.instance.Next();

        if (randomEventGeneration % 101 < 45)
        {
            CreateRandomEvent();
        }

    }

    void Update()
    {
        //print($"{energy}, {hyperFuel}");

        float newTimeScale = 1f;
        if (Input.GetKey(KeyCode.LeftControl) && energy > 0f)
        {
            energy -= 1f / Save.instance.session.skills[Skills.TimeSlowCapacity] * Time.unscaledDeltaTime;
            newTimeScale *= 0.5f;
        } 
        else if (Input.GetKey(KeyCode.LeftShift) && energy > 0f)
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

        if (Input.GetKeyDown(KeyCode.U))
        {
            for (int i = 0; i < 3; i++)
            {
                if (Mathf.Abs((ship.transform.position - planets[i].transform.position).magnitude) < 2000f)
                {
                    planetsPanels[i].gameObject.SetActive(true);
                }
            }
        }

        for (int i = 0; i < 3; i++)
        {
            planetsPanels[i].anchoredPosition = Utils.WorldToCanvasPostion(planets[i].transform.position) +
                new Vector2(Screen.width / 20f, 0);
            if (!planets[i].GetComponentInChildren<MeshRenderer>().isVisible)
            {
                planetsPanels[i].gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(hyperFuel - 1f) < 1e-5)
        {
            ship.transform.Translate(ship.transform.forward * Save.instance.session.skills[Skills.HyperDriveForce] * 100f);
            hyperFuel = 0;
        }
    }

    public static void ChangeStep()
    {
        Save.instance.session.NextStep();
        Save.Keep();
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlanetChoice");
    }

    public void CreateRandomEvent()
    {

    }

    public void ShowMessage(string text)
    {
        messageBox.gameObject.SetActive(true);
        messageBox.GetChild(1).GetComponent<Text>().text = text;
        ship.enabled = false;
        ship.rigidbody.velocity = Vector3.zero;
        StartCoroutine(WaitToButton());
    }
    public static void ShowMessageStatic(string text)
    {
        instance.ShowMessage(text);
    }
    public IEnumerator WaitToButton()
    {
        while (!Input.GetKeyDown(KeyCode.Space))
            yield return null;
        messageBox.gameObject.SetActive(false);
        ship.enabled = true;
    }
}
