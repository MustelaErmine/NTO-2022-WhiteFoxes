using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    public bool Water
    {
        get => _water;
        set {
            _water = value;
            if (!value)
            {
                GetComponentInChildren<MeshRenderer>().material.mainTexture = foodText;
                GetComponentInChildren<MeshFilter>().mesh = food;
            }
        }
    }
    bool _water;
    [SerializeField] Text text;
    int secs = 5;
    [SerializeField] ShipOnPlanet ship;
    [SerializeField] Mesh food;
    [SerializeField] Texture foodText;

    IEnumerator Catch()
    {
        ship.canMove = false;
        ship.rigidbody.velocity = Vector3.zero;
        text.gameObject.SetActive(true);
        yield return null;
        for (; secs >= 0; secs--)
        {
            text.text = "—бор ресурса. ќсталось: " + secs.ToString();
            yield return new WaitForSeconds(1);
        }
        if (Water)
        {
            Save.instance.session.inventory.Add(ItemType.Water);
        }
        else
        {
            Save.instance.session.inventory.Add(ItemType.Food);
        }
        text.gameObject.SetActive(false);
        ship.canMove = true;
        ship.UpdateWaterFood();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(Catch());
        }
    }
}
