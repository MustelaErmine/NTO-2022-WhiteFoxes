using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpaceMovePanel : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] ShipMoving ship;
    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        Vector2 pos = eventData.position;
        Vector2 screen = new Vector2(Screen.width, Screen.height);
        pos -= screen / 2;
        pos = new Vector2(Mathf.Max(Mathf.Min(pos.x / (screen.y / 2), 1f), -1f), pos.y / (screen.y / 2));
        ship.mousePosition = pos / 1f;
    }
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        ship.mousePosition = Vector2.zero;
    }
}
