using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    public GameObject spaceShip;

    [SerializeField] private GameObject sunGameobject;
    [SerializeField] private GameObject[] obstaclePrefab;
    [SerializeField] private GameObject[] orbPrefab;
    [SerializeField] private float spawnRateObstaclesPerSecond;
    [SerializeField] private float spawnRateOrbsPerSecond;
    [SerializeField] private float spawnRateIncreasePerSecond;
    [SerializeField] private float spawnDistanceFromShip;

    private float radius;
    private Coroutine coroutine1;
    private Coroutine coroutine2;
    private List<GameObject> activeItems = new List<GameObject>();

    public void StartSpawner()
    {
        RemoveAllItems();
        radius = Vector3.Distance(sunGameobject.transform.position, spaceShip.transform.position) + spawnDistanceFromShip;
        coroutine1 = StartCoroutine(SpawnObstacle(obstaclePrefab, spawnRateObstaclesPerSecond));
        coroutine2 = StartCoroutine(SpawnObstacle(orbPrefab, spawnRateOrbsPerSecond));
    }

    public void StopSpawner()
    {
        StopCoroutine(coroutine1);
        StopCoroutine(coroutine2);        
    }

    private void Update()
    {
        if (spaceShip) radius = Vector3.Distance(sunGameobject.transform.position, spaceShip.transform.position) + spawnDistanceFromShip;
    }


    IEnumerator SpawnObstacle(GameObject[] items, float spawnrate)
    {
        yield return new WaitForSeconds(spawnrate / radius);
        while (true)
        {
            // Instantiate
            float angle = Random.Range(0, 360);
            Vector3 newPos = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            GameObject go = Instantiate(items[Random.Range(0, items.Length)], newPos, Quaternion.identity, transform);

            // Assign start params
            go.GetComponent<ApplyGravitation>().sunGameobject = sunGameobject;

            // Rotate towards center
            var offset = 90f;
            Vector2 direction = sunGameobject.transform.position - go.transform.position;
            direction.Normalize();
            float rotAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            go.transform.rotation = Quaternion.Euler(Vector3.forward * (rotAngle + offset));

            // track in list
            activeItems.Add(go);
            spawnrate += spawnRateIncreasePerSecond;
            yield return new WaitForSeconds(1/(spawnrate / radius));
        }
    }

    void RemoveAllItems()
    {
        if (activeItems.Count > 0)
        {
            foreach (GameObject go in activeItems)
            {
                Destroy(go);
            }
        }
    }

}
