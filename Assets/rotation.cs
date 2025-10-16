using UnityEngine;

public class rotation : MonoBehaviour
{
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        Vector2 lookDir = player.position - transform.position;

        float angle = Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, -angle );
    }
}
