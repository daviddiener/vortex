using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 speed;
    [SerializeField] private GameObject blackHoleObjectInstantiated;
    [SerializeField] private ParticleSystem[] particleSystem;

    [SerializeField] private Sprite spaceShipTop;
    [SerializeField] private Sprite spaceShipLeft;
    [SerializeField] private Sprite spaceShipRight;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer spriteRendererShields;
    [SerializeField] private Text shieldsCounter;

    private int shieldCount = 3;

    private void Start()
    {
        shieldsCounter.text = "Shields: " + shieldCount;
    }

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        foreach (ParticleSystem p in particleSystem )
        {
            if (inputY > 0) p.startLifetime = 2;
            else p.startLifetime = 1;
        }

        if (inputX < 0) spriteRenderer.sprite = spaceShipLeft;
        else if (inputX > 0) spriteRenderer.sprite = spaceShipRight;
        else spriteRenderer.sprite = spaceShipTop;


        Vector3 movement = new Vector3(speed.x * inputX, speed.y * inputY, 0);

        movement *= Time.deltaTime;

        transform.Translate(movement);

        var offset = 90f;
        Vector2 direction = blackHoleObjectInstantiated.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        shieldCount--;
        if (shieldCount > 0) shieldsCounter.text = "Shields: " + shieldCount;
        if (collider.GetComponent<ApplyGravitation>())Destroy(collider.gameObject);

        if (shieldCount == 0) spriteRendererShields.gameObject.SetActive(false);
        if (shieldCount < 0) Destroy(gameObject);
    }
}
