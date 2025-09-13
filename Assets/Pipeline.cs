using System.Collections.Generic;
using UnityEngine;

public class Pipeline : MonoBehaviour
{
    public Camera cam;
    private Vector2 startP;
    public float spa = 0.1f; // 每隔 0.1 秒记录点
    private float timer = 0f;

    private List<Vector2> poi = new List<Vector2>();
    private bool draw = false;
    private Vector2 mousepos()
    {
        Vector3 m = cam.ScreenToWorldPoint(Input.mousePosition);
        return new Vector2(m.x, m.y);
    }

    void Start()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startP = cam.ScreenToWorldPoint(Input.mousePosition);
            poi.Clear();
            timer = 0f;
            draw = true;
            Vector2 startPos = mousepos();
            poi.Add(startPos);
        }
        if (Input.GetMouseButton(0) && draw)
        {
            timer += Time.deltaTime;
            if (timer >= spa)
            {
                timer = 0f;
                Vector2 newPos = mousepos();
                Vector2 lastPos = poi[poi.Count - 1];
                Debug.DrawLine(lastPos, newPos, Color.black, 999f);
                poi.Add(newPos);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            float lenth = 0f;
            if (poi.Count < 2)
            {
                lenth = 0f;
            }

            if (poi.Count >= 2)
            {
                Vector2 delta = poi[poi.Count - 1] - poi[poi.Count - 2];
                lenth += delta.magnitude;
            }

            Debug.Log("Pipeline length is" + lenth);
        }
    }
 
}

