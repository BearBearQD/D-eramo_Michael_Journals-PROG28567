using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float Maxfloatdistance = 20;
    public float speed = 20;
    public float arrivalDistance = 0.5f;

    public Vector3 targetpoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        newtargetpoint();
    }
    
    // Update is called once per frame
    void Update()
    {
        toward_target();
    }

    void toward_target()
    {
        Vector3 direction = (targetpoint - transform.position).normalized;
        transform.Translate(direction*speed* Time.deltaTime);
        if (Vector3.Distance(transform.position,targetpoint)<=arrivalDistance)
        {
            newtargetpoint();
        }
    }
    void newtargetpoint()
    {
        float X = Random.Range(-Maxfloatdistance, Maxfloatdistance);
        float Y = Random.Range(-Maxfloatdistance, Maxfloatdistance);
        targetpoint = new Vector3(X, Y, 0f )+ transform.position;
    }
}
