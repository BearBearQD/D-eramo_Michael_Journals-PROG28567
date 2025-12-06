using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DeSpawner : MonoBehaviour
{
    public float DespawnTime = 3f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(this.gameObject, DespawnTime);
    }
}
