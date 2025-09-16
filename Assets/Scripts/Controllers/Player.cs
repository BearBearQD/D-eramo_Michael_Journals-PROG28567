using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;

    public float BombTrailSpacing = 3f;
    public int numberOfTrailBombs = 4;

    public float spawnDistance;

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

}
