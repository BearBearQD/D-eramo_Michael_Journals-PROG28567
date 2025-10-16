using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    private float deceleration = 1f;
    public Vector2 lastDirection = Vector2.zero;

    public float radarRadius = 5f;
    public int circlepoints = 8;

    public GameObject powerupprefabs;
    public float powerupradius = 4f;
    public int numpowerupradius = 4;
    private List<GameObject> spawnpower = new List<GameObject>();

    public GameObject bulletPrefab;
    public Transform shootpoint;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;

    public int magazineSize = 10;
    private int currentAmmo;

    private void Start()
    {
        currentAmmo = magazineSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnBombAtOffset(new Vector3(0, 1));
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            SpawnBombTrail(BombTrailSpacing, numberOfTrailBombs);
        }
        if (Input.GetKeyDown(KeyCode.C))
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
        if(Input.GetKey(KeyCode.R))
        {
            currentAmmo = magazineSize;
        }
        if (Input.GetButton("Fire1") && Time.time > nextFireTime && currentAmmo>0)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
            currentAmmo--;
        }
        PlayerRotation();
        Playermovement();
        EnemyRader(radarRadius, circlepoints);
        SpawnPowerup(powerupradius, numpowerupradius);
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
            if (speed == 0)
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
            speed -= deceleration;
            transform.Translate(lastDirection * speed * Time.deltaTime);
        }
        transform.Translate(movement * speed * Time.deltaTime);

    }

    public void PlayerRotation()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 lookDir = mousePos - transform.position;

        float angle = Mathf.Atan2(lookDir.x, lookDir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, -angle);
    }
    void SpawnBombAtOffset(Vector3 inOffset)
    {
        Vector2 spawnPosition = transform.position + inOffset;

        Instantiate(bombPrefab, spawnPosition, Quaternion.identity);
    }

    void SpawnBombTrail(float bombSpacing, int numberOfBombs)
    {
        if (bombPrefab == null) return;

        for (int i = 0; i < numberOfBombs; i++)
        {
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y - bombSpacing * (i + 1), 0);
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

            if (dist < inMaxRange)
            {
                Vector3 dir = (asteroid.position - playerPos).normalized;

                Vector3 lineEnd = playerPos + dir * 2.5f;

                Debug.DrawLine(playerPos, lineEnd, Color.green, 10f);

            }
        }
    }

    bool Enemyin(Vector2 enemypos, Vector2[] polygen)
    {
        int cross = 0;
        int count = polygen.Length;

        for (int i = 0; i < count; i++)
        {
            Vector2 A = polygen[i];
            Vector2 B = polygen[(i + 1) % circlepoints];

            if ((enemypos.y > Mathf.Min(A.y, B.y)) && (enemypos.y <= Mathf.Max(A.y, B.y)))
            {
                float intersection = A.x + (enemypos.y - A.y) * (B.x - A.x) / (B.y - A.y);

                if (intersection > enemypos.x)
                {
                    cross++;
                }
                else if (intersection == enemypos.x)
                {
                    return true;
                }
            }
        }
        if ((cross % 2) != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void EnemyRader(float Radius, int circlepoints)
    {
        Vector2 playepos = transform.position;
        Vector2[] polygen = new Vector2[circlepoints];

        for (int i = 0; i < circlepoints; i++)
        {
            float angle = (2 * Mathf.PI / circlepoints) * i;
            polygen[i] = playepos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Radius;
        }

        bool Enemyinside = Enemyin(enemyTransform.position, polygen);

        if (Enemyinside == true)
        {
            Color color1 = Color.red;
            for (int i = 0; i < circlepoints; i++)
            {
                Vector2 p1 = polygen[i];
                Vector2 p2 = polygen[(i + 1) % circlepoints];
                Debug.DrawLine(p1, p2, color1);
            }
        }

        if (Enemyinside == false)
        {
            Color color = Color.green;
            for (int i = 0; i < circlepoints; i++)
            {
                Vector2 p1 = polygen[i];
                Vector2 p2 = polygen[(i + 1) % circlepoints];
                Debug.DrawLine(p1, p2, color);
            }
        }


    }

    void SpawnPowerup(float radius, int numberofpowerups)
    {
        Vector2 playerpos = transform.position;
        bool initialized = false;
        if (initialized == false)
        {
            for (int i = 0; i < numberofpowerups; i++)
            {
                float angle = (2 * Mathf.PI / numberofpowerups) * i;
                Vector2 spownpos = playerpos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                GameObject power = Instantiate(powerupprefabs, spownpos, Quaternion.identity);
                spawnpower.Add(power);

            }
            initialized = true;
        }

        for (int i = 0; i < spawnpower.Count; i++)
        {
            float angle = (2 * Mathf.PI / numberofpowerups) * i;
            Vector2 spownpos = playerpos + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            spawnpower[i].transform.position = spownpos;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, shootpoint.position, transform.rotation);
    }
}
