using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyGravitation : MonoBehaviour
{
    public bool isShieldOrb = false;
    public GameObject blackHoleObject;

    [SerializeField] private float scaleFactor = 1;
    [SerializeField] private float rotationSpeed = -0.5f;

    private float speedModifier = 1;

    private void Start()
    {
        speedModifier = Random.Range(0.7f, 1.3f);
    }
    void Update()
    {
        // Pull towards center
        transform.position = Vector3.MoveTowards(transform.position, blackHoleObject.transform.position, Time.deltaTime * blackHoleObject.GetComponent<BlackHoleGravitation>().gravityPull * speedModifier);

        // Rotate left
        Vector3 movement = new Vector3(rotationSpeed, 0, 0);
        movement *= Time.deltaTime;
        transform.Translate(movement);

        var offset = 90f;
        Vector2 direction = blackHoleObject.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));

        if (!isShieldOrb)
        {
            // Scale when item comes near center
            transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor) * Vector3.Distance(blackHoleObject.transform.position, transform.position) + new Vector3(1f, 1f, 1f);    
        }
    }
}
