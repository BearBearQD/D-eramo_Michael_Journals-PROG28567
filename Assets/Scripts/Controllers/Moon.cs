using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering;

public class Moon : MonoBehaviour
{
    public Transform planetTransform;
    public float radius = 4;
    public float speed = 1;
    public Transform target;

    private float currentangle = 0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        OrbitalMotion(radius, speed,target);
    }
    void OrbitalMotion(float radius, float speed, Transform target)
    {   
        Vector2 from = target.position;
        Vector2 to = transform.position;
        currentangle += speed * Time.deltaTime;
        float x = target.position.x+Mathf.Cos(currentangle)*radius;
        float y = target.position.y+Mathf.Sin(currentangle)*radius;
        transform.position = new Vector2(x, y);
        print(currentangle*Mathf.PI);
    }
}
