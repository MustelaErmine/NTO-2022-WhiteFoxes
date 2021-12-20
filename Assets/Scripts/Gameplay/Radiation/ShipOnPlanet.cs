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
    new Rigidbody rigidbody;
    new Transform transform;
    bool flagRd = false;
    [SerializeField] GameObject radiationOrigin;
    List<GameObject> radiations;
    void Start()
    {
        health = maxHealth;
        rigidbody = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
        Save.Load();

        radiations = new List<GameObject> { radiationOrigin };
        for (int i = 0; i < 11; i++)
        {
            radiations.Add(Instantiate(radiationOrigin));
        }
        for (int i = 0; i < 12; i++)
        {
            int x = ProceduralGeneration.instance.Next();
            int y = ProceduralGeneration.instance.Next();
            if (x % 5 == 0 && y % 5 == 0)
                continue;
            radiations[i].transform.position = new Vector3((x % 5 - 2) * 10, 0.5f, (y % 5 - 2) * 10);
            //radiations[i].GetComponentInChildren<RadiationSource>().CreateTexture();
        }
    }
    void LateUpdate()
    {
        healthSlider.value = health / maxHealth;
        if (!flagRd)
        {
            flagRd = true;
            for (int i = 0; i < 12; i++)
            {
                radiations[i].GetComponentInChildren<RadiationSource>().CreateTexture();
            }
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
            Save.instance.session = null;
            Save.Keep();
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
