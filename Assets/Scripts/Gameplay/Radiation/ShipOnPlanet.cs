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
    List<GameObject> radiations;
    [SerializeField] Text food, water;
    [SerializeField] Mesh coal;
    void Start()
    {
        health = maxHealth;
        rigidbody = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
        Save.Load();

        for (int i = -20; i <= 20; i += 10)
        {
            for (int j = -20; j <= 20; j += 10)
            {
                if (!(i == -20 && j == -20 || i == 0 && j == -20 || i == 0 && j == 0))
                {
                    int wat = ProceduralGeneration.instance.Next();
                    switch(wat % 3)
                    {
                        case 0:
                            Instantiate(radiationOrigin, new Vector3(i, 0.5f, j), new Quaternion(0,0,0,0));
                            break;
                        case 1:
                            Instantiate(enemyor, new Vector3(i, 0.5f, j), new Quaternion(0,0,0,0));
                            break;
                        case 2:
                            Resource res =  Instantiate(resor, new Vector3(i, 0.5f, j), new Quaternion(0,0,0,0))
                                .GetComponent<Resource>();
                            res.water = wat % 2 == 0;
                            if (!res.water)
                                res.GetComponentInChildren<MeshFilter>().mesh = coal;
                            break;
                    }
                }
            }
        }

        UpdateWaterFood();
    }

    internal void UpdateWaterFood()
    {
        food.text = "Еда: " + Save.instance.session.inventory.FindAll((ItemType t) => t == ItemType.Food).Count.ToString();
        water.text = "Вода: " + Save.instance.session.inventory.FindAll((ItemType t) => t == ItemType.Water).Count
             .ToString();
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
        if (Input.GetKey(KeyCode.W))
            newVelocity.z += speed;
        if (Input.GetKey(KeyCode.S))
            newVelocity.z -= speed;
        if (Input.GetKey(KeyCode.A))
            newVelocity.x -= speed;
        if (Input.GetKey(KeyCode.D))
            newVelocity.x += speed;
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
        ShowMessage("Вы умерли от недостатка здоровья", () =>
        {
            Save.Die();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        });
    }
    public void ShowMessage(string text, Action action)
    {
        messageBox.gameObject.SetActive(true);
        messageBox.GetChild(1).GetComponent<Text>().text = text;
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
}
