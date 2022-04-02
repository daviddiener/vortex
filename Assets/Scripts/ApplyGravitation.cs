using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyGravitation : MonoBehaviour
{
    [SerializeField] private bool isSpaceShip = false;
    [SerializeField] private float scaleFactor = 1;
    public GameObject blackHoleObject;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, blackHoleObject.transform.position, Time.deltaTime * blackHoleObject.GetComponent<BlackHoleGravitation>().gravityPull);

        if (!isSpaceShip) transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor) * Vector3.Distance(blackHoleObject.transform.position, transform.position) + new Vector3(1f, 1f, 1f);
    }
}
