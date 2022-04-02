using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    [SerializeField] private GameObject blackHoleObjectInstantiated;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private float spawnRate;
    [SerializeField] private float radius;

    private MonoBehaviour repeator;


    private void Start()
    {
        StartCoroutine(SpawnObstacle());
    }


    IEnumerator SpawnObstacle()
    {
        yield return new WaitForSeconds(spawnRate);
        while (true)
        {
            float angle = Random.Range(0, 360);
            Vector3 newPos = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            GameObject go = Instantiate(obstaclePrefab, newPos, Quaternion.identity, transform);
            go.GetComponent<ApplyGravitation>().blackHoleObject = blackHoleObjectInstantiated;
            yield return new WaitForSeconds(spawnRate);
        }
    }

}
