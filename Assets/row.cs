using System.Threading;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class row : MonoBehaviour
{
    public Camera cam;
    public Button button;
    public TMP_InputField InputF;
    public float square = 1f;
    public float space= 0.2f;
    public Vector3 startPosition = new Vector3(0f, 0f, 0f); 
    public float Durat = 999f;
    void squarerow()
    {
        int count;
        if(!int.TryParse(InputF.text,out count)|| count < 0)
        {
            print("wrong input");
            return;
        }
        for (int i = 0; i < count; i++)
        {
            float step = square + space;
            Vector3 center = startPosition + new Vector3(i * step, 0f, 0f);
            DrawSquare(center, square, Color.white, Durat);
        }
    }
    void DrawSquare(Vector3 center, float size, Color color, float durat)
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
        if (button != null)
          {
            button.onClick.AddListener(squarerow);
           }
        

    }
}
