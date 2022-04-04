using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleGravitation : MonoBehaviour
{
    public float initGravityPull = 1;
    public float lerpSpeed = 10;
    public float sizeScaleFactor = 0.01f;
    public float gravityScaleFactor = 0.001f;
    public AudioManager audioManager;

    [HideInInspector] public float gravityPull;

    private void Start()
    {
        resetSun();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //audioManager.playPopSound();
        Destroy(collider.gameObject);

        transform.localScale += new Vector3(1, 1, 1) * sizeScaleFactor;
        gravityPull += gravityScaleFactor;
    }

    public void resetSun()
    {
        gravityPull = initGravityPull;
        StartCoroutine(LerpBack());
    }

    public IEnumerator LerpBack()
    {
        Vector3 initialScale = transform.localScale;

        float progress = 0;

        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(initialScale, new Vector3(1, 1, 1), progress);
            progress += Time.deltaTime * lerpSpeed;
            yield return null;
        }
        transform.localScale = new Vector3(1, 1, 1);

    }

}
