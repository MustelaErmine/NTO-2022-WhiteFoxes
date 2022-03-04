using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEffect : MonoBehaviour
{
    [SerializeField] GameObject element;
    public void Play()
    {
        for (int i = 0; i < 360; i += 10)
        {
            Instantiate(element, transform).transform.eulerAngles = new Vector3(90, i, 0);
        }
    }
}
