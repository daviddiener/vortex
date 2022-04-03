using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text shieldsCounter;
    public GameObject sunGameobject;
    public MenuManager menuManager;

    [SerializeField] private Vector2 speed;
    [SerializeField] private new ParticleSystem[] particleSystem;
    [SerializeField] private Sprite spaceShipTop;
    [SerializeField] private Sprite spaceShipLeft;
    [SerializeField] private Sprite spaceShipRight;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer spriteRendererShields;

    private int shieldCount = 3;

    private void Start()
    {
        shieldsCounter.text = "Shields: " + shieldCount;
    }

    void Update()
    {
        // Pull towards center
        transform.position = Vector3.MoveTowards(transform.position, sunGameobject.transform.position, Time.deltaTime * sunGameobject.GetComponent<BlackHoleGravitation>().gravityPull);

        // Calc movement
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(speed.x * inputX, speed.y * inputY, 0);
        movement *= Time.deltaTime;
        transform.Translate(movement);

        // Rotate
        var offset = 90f;
        Vector2 direction = sunGameobject.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * (angle + offset));

        //Animation
        foreach (ParticleSystem p in particleSystem)
        {
            var main = p.main;
            if (inputY > 0) main.startLifetime = 2;
            else main.startLifetime = 0.5f;
        }

        if (inputX < 0) spriteRenderer.sprite = spaceShipLeft;
        else if (inputX > 0) spriteRenderer.sprite = spaceShipRight;
        else spriteRenderer.sprite = spaceShipTop;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Orb collision
        ApplyGravitation ag = collider.GetComponent<ApplyGravitation>();
        if (ag != null && ag.isShieldOrb)
        {
            Destroy(collider.gameObject);
            shieldCount++;
            if (shieldCount > 0) shieldsCounter.text = "Shields: " + shieldCount;
            if (shieldCount == 1) spriteRendererShields.gameObject.SetActive(true);

            return;
        }

        // Meteor collision
        if (collider.GetComponent<ApplyGravitation>())
        {
            Destroy(collider.gameObject);
            shieldCount--;
            if (shieldCount >= 0) shieldsCounter.text = "Shields: " + shieldCount;
            if (shieldCount == 0) spriteRendererShields.gameObject.SetActive(false);

            // Death
            if (shieldCount < 0) ShipGameOver();

            return;
        }
        
        // Sun collision Death
        if (collider.GetComponent<BlackHoleGravitation>()) ShipGameOver();
    }

    public void ShipGameOver()
    {
        Destroy(gameObject);
        menuManager.GameOver();
    }

}
