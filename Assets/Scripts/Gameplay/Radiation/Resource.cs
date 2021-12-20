using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resource : MonoBehaviour
{
    public bool water;
    [SerializeField] Text text;
    int secs = 5;
    [SerializeField] ShipOnPlanet ship;

    IEnumerator Catch()
    {
        ship.enabled = false;
        ship.rigidbody.velocity = Vector3.zero;
        text.gameObject.SetActive(true);
        yield return null;
        for (; secs >= 0; secs--)
        {
            text.text = "—бор ресурса. ќсталось: " + secs.ToString();
            yield return new WaitForSeconds(1);
        }
        if (water)
        {
            Save.instance.session.inventory.Add(ItemType.Water);
        }
        else
        {
            Save.instance.session.inventory.Add(ItemType.Food);
        }
        text.gameObject.SetActive(false);
        ship.enabled = true;
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
