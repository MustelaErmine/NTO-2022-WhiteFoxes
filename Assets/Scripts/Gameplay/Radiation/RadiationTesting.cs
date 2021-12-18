using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiationTesting : MonoBehaviour
{
    Texture2D _texture;
    int width = 256, height = 256;
    int center_x, center_y;
    int radius;
    private void Update()
    {
        if (_texture == null)
        {
            _texture = new Texture2D(width, height);
            _texture.filterMode = FilterMode.Point;
            GetComponent<MeshRenderer>().material.mainTexture = _texture;
        }

        radius = (width + height) / 4;
        center_x = width / 2;
        center_y = height / 2;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if ((i - center_x) * (i - center_x) + (j - center_y) * (j - center_y) <= radius * radius)
                {
                    _texture.SetPixel(i, j, new Color(0.1f, 0.2f, 0.9f, 1f));
                }
                else
                {
                    _texture.SetPixel(i, j, new Color(0f, 0f, 0f, 0f));
                }
            }
        }

        _texture.Apply();
    }
}
