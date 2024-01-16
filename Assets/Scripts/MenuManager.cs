using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityToolbag;

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
    [SerializeField] private Score score;
    [SerializeField] private AudioManager audioManager;

    [Header("References - UI")]
    [SerializeField] private GameObject counterParent;
    [SerializeField] private Text shieldsCounter;
    [SerializeField] private Text bonusCounter;
    [SerializeField] private Text timeCounter;
    [SerializeField] private GameObject selectionUI;
    [SerializeField] private GameObject gameOverParent;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text username;
    [SerializeField] private Button submitButton;

    [Header("Score-Prefabs")]
    public GameObject[] scorePrefab1;
    public GameObject[] scorePrefab2;
    public GameObject[] scorePrefab3;
    public GameObject[] scorePrefab4;
    public GameObject[] scorePrefab5;
    
    private List<GameObject[]> scorePrefabs = new List<GameObject[]>();
    private int bonusPoints = 0;
    private float timer = 0;
    private bool countTime = false;
    private int scorePage = 1;
    private const string backend_URI = "https://vortex.daviddiener.de/scores";

    void Start() {
        AddBonusPoints(0);
        scorePrefabs.Add(scorePrefab1);
        scorePrefabs.Add(scorePrefab2);
        scorePrefabs.Add(scorePrefab3);
        scorePrefabs.Add(scorePrefab4);
        scorePrefabs.Add(scorePrefab5);
        LoadScores();
    }

    void Update() {
        if (countTime) {
            timer += Time.deltaTime;
            TimeSpan ts = TimeSpan.FromSeconds(timer);
            timeCounter.text = "Time: " + ts.Minutes + ":" + ts.Seconds + ":" + ts.Milliseconds;
        }
    }

    public void SelectShip(int selection) {
        // start time count
        countTime = true;

        // reset points
        timer = 0;
        bonusPoints = 0;
        AddBonusPoints(0); // reset UI

        // Instantiate ship
        GameObject ship = Instantiate(shipPrefabs[selection], spawnLocation, Quaternion.identity, entityParent);
        
        // Set references
        PlayerController pc = ship.GetComponent<PlayerController>();
        pc.shieldsCounter = shieldsCounter;
        pc.sunGameobject = sunGameobject;
        pc.menuManager = this;
        pc.audioManager = audioManager;

        cameraFollow.Target = ship.transform;
        cameraFollow.useSmoothTimeShip();
        cameraFollow.followRotation = true;

        // Start spawner
        spawner.spaceShip = ship;
        spawner.StartSpawner();

        // Dis-/Enable UI Elements
        counterParent.SetActive(true);
        selectionUI.SetActive(false);
        gameOverParent.SetActive(false);

        // Play sound
        audioManager.playGameStart();
    }

    public void GameOver() {
        // Center camera
        cameraFollow.Target = sunGameobject.transform;
        cameraFollow.useSmoothTimeSun();
        cameraFollow.followRotation = false;

        // Reset sun
        sunGameobject.GetComponent<BlackHoleGravitation>().resetSun();

        // Stop spawner
        spawner.StopSpawner();

        // UI Elements
        StartCoroutine(LoadScores());
        submitButton.enabled = true;
        counterParent.SetActive(false);
        selectionUI.SetActive(true);
        gameOverParent.SetActive(true);

        //show points
        countTime = false;
        TimeSpan ts = TimeSpan.FromSeconds(timer);
        scoreText.text = String.Format("Time: {0} + Bonus points: {1} = Score: {2}", ts.Minutes + ":" + ts.Seconds + ":" + ts.Milliseconds, bonusPoints, ts.TotalSeconds+bonusPoints);
    }

    public void AddBonusPoints(int value) {
        bonusPoints += value;
        bonusCounter.text = "Bonus points: " + bonusPoints.ToString();
    }

    public void SubmitScore(){
        StartCoroutine(SubmitScoreRequest());    
    }

    public IEnumerator SubmitScoreRequest(){
        audioManager.playClickSound();
        submitButton.enabled = false;
        TimeSpan ts = TimeSpan.FromSeconds(timer);
        Score newScore = new Score();
        newScore.username = username.text;
        newScore.score = ts.TotalSeconds+bonusPoints;

        
        string json = JsonUtility.ToJson(newScore);
        var req = new UnityWebRequest(backend_URI, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();

        // if (req.result == UnityWebRequest.Result.ConnectionError)
        // {
        //     Debug.Log("Error While Sending: " + req.error);
        // }
        // else
        // {
        //     Debug.Log("Received: " + req.downloadHandler.text);
        // }
        
        StartCoroutine(LoadScores());    
    }

    public void LoadNextScores(){
        audioManager.playClickSound();
        scorePage++;
        StartCoroutine(LoadScores());    
    }

    public void LoadPreviousScores(){
        audioManager.playClickSound();
        if (scorePage > 1) {
            scorePage--;
        StartCoroutine(LoadScores());        
        }   
    }

    public IEnumerator LoadScores(){
        UnityWebRequest request = UnityWebRequest.Get(backend_URI+"?pageLimit="+scorePrefabs.Count+"&pageNum="+scorePage);
        yield return request.SendWebRequest();
        Score[] scoreList = JsonHelper.FromJson<Score>(request.downloadHandler.text);

        // abort if page empty
        if(scoreList.Length < 1) {
            if (scorePage > 1) {
                scorePage--;
            }
            yield break;
        }

        // Place - everywhere if page not full
        if(scoreList.Length < scorePrefabs.Count) {
            for(int i = 0; i < scorePrefabs.Count; ++i) {
                scorePrefabs[i][0].GetComponent<Text>().text = ((i+1)+((scorePage-1)*scorePrefabs.Count)).ToString();
                scorePrefabs[i][1].GetComponent<Text>().text = "-";
                scorePrefabs[i][2].GetComponent<Text>().text = "-";
            }
            
        }

        // populate board
        for(int i = 0; i < scoreList.Length; ++i) {
            scorePrefabs[i][0].GetComponent<Text>().text = ((i+1)+((scorePage-1)*scorePrefabs.Count)).ToString();
            scorePrefabs[i][1].GetComponent<Text>().text = scoreList[i].username;
            scorePrefabs[i][2].GetComponent<Text>().text = scoreList[i].score.ToString();
        }
    }
    
}