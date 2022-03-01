using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighSpeedButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] SpaceControl control;
    public void OnPointerDown(PointerEventData eventData)
    {
        control.highPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        control.highPressed = false;
    }
}
