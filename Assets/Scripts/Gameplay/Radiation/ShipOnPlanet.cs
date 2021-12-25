using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipOnPlanet : MonoBehaviour
{
    const float maxHealth = 100f;
    float health;
    [SerializeField] Slider healthSlider;
    [SerializeField] RectTransform messageBox;
    float speed = 10f;
    public new Rigidbody rigidbody;
    new Transform transform;
    bool flagRd = false;
    [SerializeField] GameObject radiationOrigin, enemyor, resor;
    [SerializeField] Text food, water, years;
    [SerializeField] Mesh coal;
    [SerializeField] AudioClip notif;
    [SerializeField] Texture[] floorTiles;
    [SerializeField] MeshRenderer floor;
    [SerializeField] Mesh[] crystals;
    [SerializeField] Texture[] crystalsTexts;
    [SerializeField] MeshRenderer crystal;
    public bool canMove = true;

    void Start()
    {
        health = maxHealth;
        rigidbody = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
        Save.Load();
        List<int> map = new List<int>(25);
        for (int i = 0; i < 25; i++)
        {
            if (i < 5)
                map.Add(2);
            else if (i < 15)
                map.Add(1);
            else
                map.Add(0);
        }
        map.Shuffle();
        for (int i = 1; i < 24; i++)
        {
            if (map[i] == 0)
                continue;
            int x = i % 5 * 10 - 20, y = i / 5 * 10 - 20;
            GameObject orig = null;
            if (map[i] == 2)
                orig = resor;
            else
                orig = radiationOrigin;
            GameObject gm = Instantiate(orig, new Vector3(x, 0.5f, y), new Quaternion());
            if (map[i] == 2)
            {
                gm.GetComponent<Resource>().Water = i % 2 == 0;
            }
        }

        UpdateWaterFood();

        floor.material.mainTexture = floorTiles[PlanetControl.useType];
        crystal.material.mainTexture = crystalsTexts[PlanetControl.useType];
        crystal.GetComponent<MeshFilter>().mesh = crystals[PlanetControl.useType];
        enemyor.SetActive(false);

        StartCoroutine(Enemies());
    }

    internal void UpdateWaterFood()
    {
        food.text = Save.instance.session.inventory.FindAll((ItemType t) => t == ItemType.Food).Count.ToString();
        water.text = Save.instance.session.inventory.FindAll((ItemType t) => t == ItemType.Water).Count.ToString();
        years.text = Save.instance.session.years.ToString();
    }

    void LateUpdate()
    {
        healthSlider.value = health / maxHealth;
        if (!flagRd)
        {
            flagRd = true;
            radiationOrigin.GetComponentInChildren<RadiationSource>().CreateTexture();

        }
    }
    private void FixedUpdate()
    {
        Vector3 newVelocity = Vector3.zero;
        if (canMove)
        {
            if (Input.GetKey(KeyCode.W))
                newVelocity.z += speed;
            if (Input.GetKey(KeyCode.S))
                newVelocity.z -= speed;
            if (Input.GetKey(KeyCode.A))
                newVelocity.x -= speed;
            if (Input.GetKey(KeyCode.D))
                newVelocity.x += speed;
        }
        rigidbody.velocity = newVelocity;
    }
    public void ApplyRadiation(float rad)
    {
        health -= rad;
        if (health <= 0f)
        {
            Die();
        }
    }
    public void Die()
    {
        Save.Die();
        ShowMessage("Вы умерли от недостатка здоровья", () =>
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        });
    }
    public void ShowMessage(string text, Action action)
    {
        messageBox.gameObject.SetActive(true);
        messageBox.GetChild(1).GetComponent<Text>().text = text;
        messageBox.GetComponent<AudioSource>().PlayOneShot(notif);
        enabled = false;
        rigidbody.velocity = Vector3.zero;
        StartCoroutine(WaitToButton(action));
    }
    public IEnumerator WaitToButton(Action action)
    {
        while (!Input.GetKeyDown(KeyCode.C))
            yield return null;
        messageBox.gameObject.SetActive(false);
        enabled = true;
        yield return new WaitForSeconds(0.01f);
        action?.Invoke();
    }
    public IEnumerator Enemies()
    {
        while (true)
        {
            int r = new System.Random().Next(0, 3);
            float x = 0, y = 0;
            if (r == 0)
            {
                x = 5;
                y = 5;
            }
            else if (r == 1)
            {
                x = 5;
                y = -5;
            }
            else if (r == 2)
            {
                x = -5;
                y = 5;
            }
            else
            {
                x = -5;
                y = -5;
            }
            Instantiate(enemyor, new Vector3(x, 0.5f, y), new Quaternion()).SetActive(true);
            yield return new WaitForSeconds(2f);
        }
    }
}
