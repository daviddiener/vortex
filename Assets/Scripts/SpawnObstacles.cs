using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    [SerializeField] private GameObject blackHoleObjectInstantiated;
    [SerializeField] private GameObject spaceShip;
    [SerializeField] private GameObject[] obstaclePrefab;
    [SerializeField] private GameObject[] orbPrefab;
    [SerializeField] private float spawnRateObstacles;
    [SerializeField] private float spawnRateOrbs;
    [SerializeField] private float spawnDistanceFromShip;

    private float radius;


    private void Start()
    {
        radius = Vector3.Distance(blackHoleObjectInstantiated.transform.position, spaceShip.transform.position) + spawnDistanceFromShip;
        StartCoroutine(SpawnObstacle(obstaclePrefab, spawnRateObstacles));
        StartCoroutine(SpawnObstacle(orbPrefab, spawnRateOrbs));
    }

    private void Update()
    {
        if (spaceShip) radius = Vector3.Distance(blackHoleObjectInstantiated.transform.position, spaceShip.transform.position) + spawnDistanceFromShip;
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
            go.GetComponent<ApplyGravitation>().blackHoleObject = blackHoleObjectInstantiated;

            // Rotate towards center
            var offset = 90f;
            Vector2 direction = blackHoleObjectInstantiated.transform.position - go.transform.position;
            direction.Normalize();
            float rotAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            go.transform.rotation = Quaternion.Euler(Vector3.forward * (rotAngle + offset));

            yield return new WaitForSeconds(spawnrate / radius);
        }
    }

}
