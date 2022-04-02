using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyGravitation : MonoBehaviour
{
    [HideInInspector] public GameObject blackHoleObject;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, blackHoleObject.transform.position, Time.deltaTime * blackHoleObject.GetComponent<BlackHoleGravitation>().gravityPull);
    }
}
