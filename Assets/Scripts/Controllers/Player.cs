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

    // Update is called once per frame
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.B))
        {
            SpawnBombAtOffset(new Vector3(0, 1));
        }

        void SpawnBombAtOffset(Vector3 inOffset)
        {
            Vector2 spawnPosition = transform.position + inOffset;

            Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
        }


    }
}
