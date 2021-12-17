using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceControl : MonoBehaviour
{
    public PlanetControl[] planets;
    public Slider energySlider, fuelSlider;
    public ShipMoving ship;

    public float energy = 0;
    public float hyperFuel = 0;

    void Start()
    {
        if (Save.instance.session == null)
        {
            Save.instance.session = new Session();
        }
        Save.Load();
        for (byte i = 0; i < 3; i++)
            planets[i].GenerationNumber = ProceduralGeneration.instance.Next();

        Application.targetFrameRate = 60;

        energy = 1;
        hyperFuel = 1;
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
        UnityEngine.SceneManagement.SceneManager.LoadScene("PlanetChoice");
    }
}
