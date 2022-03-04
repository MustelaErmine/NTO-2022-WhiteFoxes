using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] AudioClip btnclp;
    static AudioSource source;
    Vector3 scale;
    void Start()
    {
        scale = GetComponent<RectTransform>().localScale;
        try
        {
            if (source == null)
            {
                source = GameObject.Find("BtnSource").GetComponent<AudioSource>();
            }
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if (source != null)
                    source.PlayOneShot(btnclp);
            });
        } catch (System.Exception e)
        {
            print("no btn source");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<RectTransform>().localScale = Vector3.one * 1.1f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GetComponent<RectTransform>().localScale = scale;
    }
}
