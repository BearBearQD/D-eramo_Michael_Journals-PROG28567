using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;
    public float maxRange = 10f;

    public float BombTrailSpacing = 3f;
    public int numberOfTrailBombs = 4;
    public float spawnDistance = 1;
    public float warpRatio = 0.5f;

    public float accleartion = 0.7f;
    public float MAXSPEED = 30;
    public float speed = 0.5f;
    public float deceleration = 0.0001f;
    public Vector2 lastDirection = Vector2.zero;





    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.B))
        {
            SpawnBombAtOffset(new Vector3(0, 1));
        }

        if(Input.GetKeyDown(KeyCode.T))
        {
            SpawnBombTrail(BombTrailSpacing, numberOfTrailBombs);
        }
        if(Input.GetKeyDown(KeyCode.C))
        {
            SpawnBombAtRandomCorner(spawnDistance);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            WarpTowardsTarget(enemyTransform, warpRatio);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            DrawAstroidLines(asteroidTransforms, maxRange);
        }
        Playermovement();

    }
    public void Playermovement()
    {
        Vector2 movement = Vector2.zero;
        if (Input.GetKey("up"))
        {
            movement += Vector2.up;
        }
        if (Input.GetKey("down"))
        {
            movement += Vector2.down;
        }
        if (Input.GetKey("right"))
        {
            movement += Vector2.right;
        }
        if (Input.GetKey("left"))
        {
            movement += Vector2.left;
        }
        if (movement != Vector2.zero && speed <= MAXSPEED)
        {
            if (speed ==0)
            {
                speed = 0.5f;
            }
            else
            {
                movement = movement.normalized;
                lastDirection = movement;
                speed += accleartion;
                print(speed);
            }
        }
        else if (movement == Vector2.zero && speed > 0.5f)
        {
            speed -= deceleration ;
           transform.Translate(lastDirection * speed * Time.deltaTime);
        }
        transform.Translate(movement * speed * Time.deltaTime);

    }

    void SpawnBombAtOffset(Vector3 inOffset)
    {
        Vector2 spawnPosition = transform.position + inOffset;

        Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
    }

    void SpawnBombTrail(float bombSpacing, int numberOfBombs)
    {
        if (bombPrefab == null) return;

        for(int i = 0; i < numberOfBombs; i++)
        {
            Vector3 spawnPos = new Vector3 (transform.position.x, transform.position.y - bombSpacing * (i + 1), 0);
            Instantiate(bombPrefab, spawnPos, Quaternion.identity);
            Debug.Log(i);
        }
    }

    void SpawnBombAtRandomCorner(float inDistance)
    {
        if (bombPrefab == null) return;

        Vector3[] cornerOffsets = new Vector3[]
        {
            new Vector3(-1,1,0), // Top left
            new Vector3(1,1,0), // Top right
            new Vector3(-1,-1,0), // Bottom left
            new Vector3(1,-1,0) // Bottom right
        };

        int randomCornerPick = Random.Range(0, cornerOffsets.Length);
        Vector3 chosenDirection = cornerOffsets[randomCornerPick].normalized;

        Vector3 spawnPos = transform.position + chosenDirection * inDistance;

        Instantiate(bombPrefab, spawnPos, Quaternion.identity);
    }
    void WarpTowardsTarget(Transform target, float ratio)
    {
        ratio = Mathf.Clamp01(ratio);

        Vector3 startPos = transform.position;
        Vector3 targetPos = target.position;

        Vector3 newPos = Vector3.Lerp(startPos, targetPos, ratio);

        transform.position = newPos;
    }

    void DrawAstroidLines(List<Transform> inAsteroids, float inMaxRange)
    {
        if (inAsteroids == null || inAsteroids.Count == 0) return;

        Vector3 playerPos = transform.position;

        foreach (Transform asteroid in inAsteroids)
        {

            float dist = Vector3.Distance(playerPos, asteroid.position);

            if(dist < inMaxRange)
            {
                Vector3 dir = (asteroid.position - playerPos).normalized;

                Vector3 lineEnd = playerPos + dir * 2.5f;

                Debug.DrawLine(playerPos, lineEnd, Color.green, 10f);

            }
        }
    }
}
