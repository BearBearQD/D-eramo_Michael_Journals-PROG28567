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

}
