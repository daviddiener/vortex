using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject sunGameobject;
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private Text shieldsCounter;
    [SerializeField] private GameObject selectionUI;
    [SerializeField] private GameObject deadText;
    [SerializeField] private GameObject[] shipPrefabs;
    [SerializeField] private Transform entityParent;
    [SerializeField] private Vector3 spawnLocation;

    [SerializeField] private SpawnObstacles spawner;

    public void SelectShip(int selection)
    {
        // Instantiate ship
        GameObject ship = Instantiate(shipPrefabs[selection], spawnLocation, Quaternion.identity, entityParent);
        
        // Set references
        PlayerController pc = ship.GetComponent<PlayerController>();
        pc.shieldsCounter = shieldsCounter;
        pc.sunGameobject = sunGameobject;
        pc.menuManager = this;

        cameraFollow.Target = ship.transform;
        cameraFollow.useSmoothTimeShip();
        cameraFollow.followRotation = true;

        // Start spawner
        spawner.spaceShip = ship;
        spawner.StartSpawner();

        // Dis-/Enable UI Elements
        shieldsCounter.gameObject.SetActive(true);
        selectionUI.SetActive(false);
        deadText.SetActive(false);

    }

    public void GameOver()
    {
        // Center camera
        cameraFollow.Target = sunGameobject.transform;
        cameraFollow.useSmoothTimeSun();
        cameraFollow.followRotation = false;

        // Reset sun
        StartCoroutine(sunGameobject.GetComponent<BlackHoleGravitation>().LerpBack());

        // Stop and cleanup spawner
        spawner.StopSpawner();
        spawner.RemoveAllItems();
        

        shieldsCounter.gameObject.SetActive(false);
        selectionUI.SetActive(true);
        deadText.SetActive(true);
    }
}
