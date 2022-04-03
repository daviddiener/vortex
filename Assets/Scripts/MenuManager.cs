using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Header("References - Gameobjects")]
    [SerializeField] private GameObject sunGameobject;
    [SerializeField] private GameObject[] shipPrefabs;
   
    [Header("References - Scripts")]
    [SerializeField] private CameraFollow cameraFollow;
    [SerializeField] private Transform entityParent;
    [SerializeField] private Vector3 spawnLocation;
    [SerializeField] private SpawnObstacles spawner;

    [Header("References - UI")]
    [SerializeField] private GameObject counterParent;
    [SerializeField] private Text shieldsCounter;
    [SerializeField] private Text bonusCounter;
    [SerializeField] private Text timeCounter;
    [SerializeField] private GameObject selectionUI;
    [SerializeField] private GameObject deadText;
    
    private int overallPoints = 0;
    private int bonusPoints = 0;
    private float timer = 0;
    private bool countTime = false;

    void Start() {
        AddBonusPoints(0);
    }

    void Update(){
        if (countTime) {
            timer += Time.deltaTime;
            TimeSpan ts = TimeSpan.FromSeconds(timer);
            timeCounter.text = "Time: " + ts.Minutes + ":" + ts.Seconds + ":" + ts.Milliseconds;
        }
    }

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
        counterParent.SetActive(true);
        selectionUI.SetActive(false);
        deadText.SetActive(false);

        // start time count
        countTime = true;

    }

    public void GameOver()
    {
        // Center camera
        cameraFollow.Target = sunGameobject.transform;
        cameraFollow.useSmoothTimeSun();
        cameraFollow.followRotation = false;

        // Reset sun
        sunGameobject.GetComponent<BlackHoleGravitation>().resetSun();

        // Stop spawner
        spawner.StopSpawner();

        counterParent.SetActive(false);
        selectionUI.SetActive(true);
        deadText.SetActive(true);

        //Submit points and then reset
        countTime = false;
        bonusPoints = 0;
        AddBonusPoints(0); // reset UI
        overallPoints = 0;
        timer = 0;
    }

    public void AddBonusPoints(int value) {
        bonusPoints += value;
        bonusCounter.text = "Bonus points: " + bonusPoints.ToString();
    }
}
