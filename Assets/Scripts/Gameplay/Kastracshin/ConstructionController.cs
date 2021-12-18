using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ConstructionController : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public Transform mainShape;
    Vector2 startedAt;

    public void OnDrag(PointerEventData eventData)
    {
        mainShape.Rotate(mainShape.InverseTransformDirection(eventData.delta.y, -eventData.delta.x, 0f));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startedAt = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if ((eventData.position - startedAt).magnitude < 0.1f)
        {
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);
            Debug.DrawRay(ray.origin, ray.direction, Color.red);
            RaycastHit[] hits = Physics.RaycastAll(ray, 1000f);
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform.gameObject.name == "Capsule")
                {
                    GameObject prim = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    prim.transform.localScale = Vector3.one * 0.1f;
                    GameObject obj = Instantiate(prim, mainShape);
                    obj.transform.position = hit.point;
                    Destroy(prim);
                    //print(Quaternion.LookRotation(hit.normal));
                    obj.transform.rotation = Quaternion.LookRotation(hit.normal);
                }
            }
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
