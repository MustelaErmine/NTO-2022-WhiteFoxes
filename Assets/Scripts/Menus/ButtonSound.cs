using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour
{
    [SerializeField] AudioClip btnclp;
    static AudioSource source;
    void Start()
    {
        if (source == null)
        {
            source = GameObject.Find("BtnSource").GetComponent<AudioSource>();
        }
        GetComponent<Button>().onClick.AddListener(()=> {
            if (source != null)
                source.PlayOneShot(btnclp);
        });
    }
}
