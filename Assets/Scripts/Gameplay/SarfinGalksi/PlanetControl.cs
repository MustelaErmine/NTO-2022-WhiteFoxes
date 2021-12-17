using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetControl : MonoBehaviour
{
    public int GenerationNumber 
    {
        set
        {
            _generationNumber = value;
            type = _generationNumber % 4;
            Color color = Color.black;

            switch (type)
            {
                case 0:
                    color = Color.red;
                    break;
                case 1:
                    color = Color.blue; 
                    break;
                case 2:
                    color = Color.green; 
                    break;
                case 3:
                    color = Color.cyan; 
                    break;
            }
            //Temp
            GetComponent<MeshRenderer>().material.color = color;
            //EndTemp
            float delta = _generationNumber % 2003 + 2000;
            Vector3 direction = new Vector3(_generationNumber % 137, _generationNumber % 139, _generationNumber % 149);
            direction = direction.normalized;

            transform.Translate(direction * delta);
        }
    }
    int _generationNumber = 0;
    int type = 0;

    new Transform transform {
        get {
            if (_transform == null)
                _transform = GetComponent<Transform>();
            return _transform;
        }
    }
    Transform _transform;
    void Awake()
    {
    }
}
