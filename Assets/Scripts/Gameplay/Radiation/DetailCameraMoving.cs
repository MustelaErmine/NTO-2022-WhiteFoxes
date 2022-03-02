using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DetailCameraMoving : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] ShipOnPlanet ship;
    Vector2 downed;
    public void OnPointerDown(PointerEventData eventData)
    {
        downed = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if ((eventData.position - downed).magnitude < 10f && ship.useDetail)
        {
            Ray ray = ship.useDetail.cam.ScreenPointToRay(eventData.position);
            ship.useDetail.MakeShot(ray.direction);
        }
    }

    public void OnDrag(PointerEventData eventData)
    { 
        if (ship.useDetail)
        {
            Vector2 scrooll = eventData.delta;
            scrooll = new Vector2(scrooll.x / Screen.width, scrooll.y / Screen.height);
            ship.useDetail.RotateCamera(scrooll);
        }
    }
}
