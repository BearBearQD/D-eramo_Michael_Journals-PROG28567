using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public List<Transform> starTransforms;
    public float drawingTime=3f;
    public float timecount=0;
    
    public int index = 0;
    public float duration = 3;

    void Update()
    {
        if (starTransforms.Count < 2)
        {
            return;
        }

        timecount += Time.deltaTime;
        float t = timecount / drawingTime;

        DrawStar(index, t);

        if (t >= 1)
        {
            timecount = 0;
            if (index == 12)
            {
                lastDrawStar(13, t);
                index = 0;
            }
            else 
            { 
                index += 1; 
            }
        }
        
    }   
    void DrawStar(int i, float t)
    {
  
        Transform startstar= starTransforms[i];
        Transform endstar= starTransforms[i+1];

        Vector3 currentend = Vector3.Lerp(startstar.position, endstar.position, t);

        Debug.DrawLine(startstar.position, currentend, Color.yellow, duration);
    }
    void lastDrawStar(int i, float t)
    {

        Transform startstar = starTransforms[i];
        Transform endstar = starTransforms[0];

        Vector3 currentend = Vector3.Lerp(startstar.position, endstar.position, t);

        Debug.DrawLine(startstar.position, currentend, Color.yellow, duration);
    }
}
