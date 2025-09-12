using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class SquareSpawner : MonoBehaviour
{
    public Camera cam;
    public float squareSize = 0f;
    public float minsize = 0.001f;

    private List<Vector3> mouseCe = new List<Vector3>();
    private List<Vector3> mouseCeP = new List<Vector3>();
    public List<float> actsize = new List<float>();
    private void Drawing(Vector3 center, float size, Color color, float durat)

    {
        float h = size * 0.5f;
        Vector3 a = new Vector3(center.x - h, center.y - h, 0f);
        Vector3 b = new Vector3(center.x - h, center.y + h, 0f);
        Vector3 c = new Vector3(center.x + h, center.y + h, 0f);
        Vector3 d = new Vector3(center.x + h, center.y - h, 0f);

        Debug.DrawLine(a, b, color, durat);
        Debug.DrawLine(b, c, color, durat);
        Debug.DrawLine(c, d, color, durat);
        Debug.DrawLine(d, a, color, durat);
    }
    void Start()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        float scl = Input.mouseScrollDelta.y;
        if (scl != 0)
        {
            squareSize += scl * 1f+ minsize;
        }
        Vector3 m = cam.ScreenToWorldPoint(Input.mousePosition);
        m.z = 0f;
        mouseCe.Add(m);

        Drawing(m, squareSize, new Color(1f, 1f, 1f, 0.2f), 0.1f);

        if (Input.GetMouseButtonDown(0))
        {
            mouseCeP.Add(m);
            actsize.Add(squareSize);

        }
        for (int i = 0; i < mouseCeP.Count; i++)
        {
            Drawing(mouseCeP[i], actsize[i], Color.white, 9999f);
        }

    }
  

}