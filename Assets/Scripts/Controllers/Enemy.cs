using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Timeline;
using System;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float stopdistance = 4;
    public float circle_distance = 6;
    public Vector2 randomdirection;
    public Vector2 awaydirection;
    public float precentage = 100000;

    private void Update()
    {
        enemymovement();
    }

    void enemymovement()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        // print(distance);
        if (distance > circle_distance)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.Translate(dir * speed * Time.deltaTime);
        }
        if (distance <= circle_distance && distance >= stopdistance)
        {
            print("mid distance");
            if (randomdirection == Vector2.zero || Random.Range(0, 100) < 2)
            {
                randomdirection = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5)).normalized;
                print("random deir");
            }
            transform.Translate(randomdirection * speed * Time.deltaTime);
        }
        if (distance < circle_distance)
        {
            awaydirection = (transform.position - player.position).normalized;
            if (awaydirection == Vector2.zero || Random.Range(0, precentage) < 1) 
            {
                awaydirection = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            }
            transform.Translate(awaydirection * speed * Time.deltaTime);
        }
        else
        {
            print(distance);
        }
    }
}
