using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class RadiationSource : MonoBehaviour
{
    Texture2D texture;
    const int width = 256, height = 256;
    const int center_x = width / 2, center_y = height / 2;
    float scale = 10;
    const int radius = (width + height) / 4;
    float rradius;
    new Transform transform;
    Transform quad;
    Vector3? lastPosition;
    int mask;
    public float? time = null;
    [SerializeField] GameObject mePrefab;
    void Start()
    {
        texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        transform = GetComponent<Transform>();
        transform.GetChild(0).GetComponent<MeshRenderer>().material.mainTexture = texture;
        lastPosition = null;
        mask = LayerMask.GetMask("Walls");
        quad = transform.GetChild(0);
        CreateTexture();
    }

    void FixedUpdate()
    {
        scale = quad.localScale.x * transform.localScale.x;
        if (transform.parent != null)
        {
            scale *= transform.parent.localScale.x;
        }
        print(scale);
        rradius = scale / 2;
        if (transform.position != lastPosition && !time.HasValue)
            CreateTexture();
        lastPosition = transform.position;
    }

    private void Update()
    {
        if (time.HasValue && time > 0)
        {
            time -= Time.deltaTime;
        }
        if (time.HasValue && time < 0.01f)
        {
            Destroy(this);
        }
    }

    public void CreateTexture()
    {
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
                colors[i, j] = to_center;
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
                    float x, y;
                    int x1, x2, y1, y2;
                    Vector3 newPoint = p + destination * i * scale / width;
                    x = (newPoint.x / scale * width) + center_x;
                    y = (newPoint.z / scale * width) + center_y;
                    x1 = Mathf.RoundToInt(x);
                    y1 = Mathf.RoundToInt(y); 
                    if (0 <= x1 && x1 < width && 0 <= y1 && y1 < height)
                        colors[x1, y1] *= mul;
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
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Ray ray = new Ray(transform.position, other.transform.position - transform.position);
            RaycastHit[] hits = Physics.RaycastAll(ray, (other.transform.position - transform.position).magnitude, mask);
            float mul = 1f - (other.transform.position - transform.position).magnitude / rradius;
            print(mul);
            foreach (RaycastHit hit in hits)
            {
                mul *= 1f - hit.transform.GetComponent<AntiRadiationWall>().antiEffect;
            }
            other.GetComponent<ShipOnPlanet>().ApplyRadiation(mul);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Ray ray = new Ray(transform.position, other.transform.position - transform.position);
            RaycastHit[] hits = Physics.RaycastAll(ray, (other.transform.position - transform.position).magnitude, mask);
            float mul = 1f - (other.transform.position - transform.position).magnitude / rradius;
            foreach (RaycastHit hit in hits)
            {
                mul *= 1f - hit.transform.GetComponent<AntiRadiationWall>().antiEffect;
            }
            if (other.GetComponentInChildren<RadiationSource>() == null)
            {
                GameObject child = Instantiate(mePrefab, other.transform);
                child.GetComponent<RadiationSource>().time = 15f;
                Enemy enemy = other.GetComponent<Enemy>();
                enemy.normal = enemy.rad; 
                enemy.mnormal = enemy.mrad;
                enemy.GetComponent<MeshRenderer>().material = enemy.rad;
                enemy.GetComponent<MeshFilter>().mesh = enemy.mrad;
            }
        }
    }
}
