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
            // Instantiate
            float angle = Random.Range(0, 360);
            Vector3 newPos = new Vector2(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius);
            GameObject go = Instantiate(obstaclePrefab, newPos, Quaternion.identity, transform);
            
            // Assign start params
            go.GetComponent<ApplyGravitation>().blackHoleObject = blackHoleObjectInstantiated;

            // Rotate towards center
            var offset = 90f;
            Vector2 direction = blackHoleObjectInstantiated.transform.position - go.transform.position;
            direction.Normalize();
            float rotAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            go.transform.rotation = Quaternion.Euler(Vector3.forward * (rotAngle + offset));

            yield return new WaitForSeconds(spawnRate);
        }
    }

}
