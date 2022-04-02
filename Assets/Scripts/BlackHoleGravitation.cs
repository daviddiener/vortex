using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleGravitation : MonoBehaviour
{
    public float gravityPull = 10;
    public float sizeScaleFactor = 0.01f;
    public float gravityScaleFactor = 0.001f;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Destroy(collider.gameObject);

        transform.localScale += new Vector3(1, 1, 1) * sizeScaleFactor;
        gravityPull += gravityScaleFactor;
    }
}
