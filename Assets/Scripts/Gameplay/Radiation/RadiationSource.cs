using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadiationSource : MonoBehaviour
{
    Texture2D texture;
    const int width = 256, height = 256;
    const int center_x = width / 2, center_y = height / 2;
    const float scale = 10;
    const int radius = (width + height) / 4;
    const float rradius = scale / 2;
    new Transform transform;
    Vector3 lastPosition;
    int mask;
    bool isCreating = false;
    void Start()
    {
        print(rradius);
        texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Bilinear;
        transform = GetComponent<Transform>();
        transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = texture;
        lastPosition = Vector3.up;
        mask = LayerMask.GetMask("Walls");
    }

    void FixedUpdate()
    {
        if (transform.position != lastPosition && !isCreating)
            CreateTexture();
        lastPosition = transform.position;
    }

    void CreateTexture()
    {
        isCreating = true;
        Vector3 origin = transform.position;
        /*for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float x = i - center_x, y = j - center_y;
                float rx = x / width * scale, ry = y / height * scale;
                if (x * x + y * y <= radius * radius)
                {
                    Vector3 destination = new Vector3(origin.x + rx,  0.5f, origin.z + ry);
                    Ray ray = new Ray(origin, destination - origin);
                    RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Sqrt(rx * rx + ry * ry), mask);
                    float multiple = 1f;
                    foreach (RaycastHit hit in hits)
                        multiple *= 1 - hit.collider.GetComponent<AntiRadiationWall>().antiEffect;
                        
                    float to_center = radius - Mathf.Sqrt(x * x / (width * width) + y * y / (height * height));
                    texture.SetPixel(i, j, new Color(0f, multiple * to_center, 0f, multiple * to_center));
                }
                else
                {
                    texture.SetPixel(i, j, new Color(0f, 0f, 0f, 0f));
                }
            }
        }*/
        float[,] colors = new float[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                float x = i - center_x, y = j - center_y;
                float to_center = 1f - Mathf.Sqrt(x * x / (radius * radius) + y * y / (radius * radius));
                colors[i, j] = to_center * 1.5f;
                //colors[i, j] = 1f;
            }
        }
        float att = 2;
        for (int angle = 0; angle < 360 * att; angle++)
        {
            Vector3 destination = new Vector3(Mathf.Cos(angle / att), 0, Mathf.Sin(angle / att));
            float dest_len = (destination * scale / width).magnitude;
            Ray ray = new Ray(origin, destination);
            List<RaycastHit> hits = Physics.RaycastAll(ray, scale, mask).ToList();
            hits.Sort(new System.Comparison<RaycastHit>((RaycastHit hit1, RaycastHit hit2) => {
                return (int)((hit1.point - origin).magnitude - (hit2.point - origin).magnitude);
            }));
            foreach(RaycastHit hit in hits)
            {
                Vector3 p = hit.point - origin;
                float mul = hit.transform.GetComponent<AntiRadiationWall>().antiEffect;
                for (int i = 0; i < (int)((rradius - p.magnitude) / dest_len) * 2; i++)
                {
                    int x, y;
                    Vector3 newPoint = p + destination * i * scale / width;
                    x = (int)(newPoint.x / scale * width) + center_x;
                    y = (int)(newPoint.z / scale * width) + center_y;
                    if (0 <= x && x < width && 0 <= y && y < height)
                        colors[x, y] *= mul;
                }
            }
        }

        float GetDot(int x, int y)
        {
            if (x < 0 || x >= width || y < 0 || y >= width)
                return 0;
            return colors[x, y];
        }

        float GetGauss (int x, int y)
        {
            float coms = 0;
            for (int i = 0; i < 2; i++)
                for (int j = 0; j < 2; j++)
                    coms += GetDot(x + i, y + j);
            return coms / 4f;
        }

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                //float o = GetGauss(i, j);
                float o = colors[i, j];
                texture.SetPixel(i, j, new Color(0, o, 0, o));
            }
        }

        texture.Apply();
        isCreating = false;
    }
}
