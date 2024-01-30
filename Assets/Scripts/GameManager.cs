using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private CameraManager cameraManager;

    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private GameObject pinPrefab;

    [SerializeField]
    private GameObject meteorPrefab;

    [SerializeField]
    private Transform coinSpawnpointsContainer;
    private List<Transform> coinSpawnpoints = new List<Transform>();

    [SerializeField]
    private Transform playerSpawnpointsContainer;
    private List<Transform> playerSpawnpoints = new List<Transform>();
    public Transform GetRandomPlayerSpawnpoint() => playerSpawnpoints[Random.Range(0, playerSpawnpoints.Count)];

    [SerializeField]
    private Transform pinSpawnpointsContainer;
    private List<Transform> pinSpawnpoints = new List<Transform>();

    [SerializeField]
    private Transform meteorSpawnpointsContainer;
    private List<Transform> meteorSpawnpoints = new List<Transform>();

    [SerializeField]
    private float totalTime;

    public float TimeLeft { get; private set; }

    public event Action<float> OnTimeLeftChange;

    [SerializeField]
    private GameObject[] enableWhenSpeech;

    [SerializeField]
    private TMP_Text speechText;

    [Serializable]
    private class SpawnCoins {
        public float time;
        public float amount;
    }

    [SerializeField]
    private SpawnCoins[] spawnCoins;

    [Serializable]
    private class Speech {
        public float time;
        public float duration;
        public string text;
        public AudioClip sound;
    }

    [SerializeField]
    private Speech[] speech;

    [SerializeField]
    private float[] pinSpawnTimes;

    [SerializeField]
    private float[] meteorSpawnTimes;

    private void Awake() {
        if (!GameInfo.StartSceneHasLoaded) {
            GameInfo.SetPlayer(0, true);
        }

        TimeLeft = totalTime;
        foreach (Transform child in coinSpawnpointsContainer) {
            coinSpawnpoints.Add(child);
        }
        foreach (Transform child in playerSpawnpointsContainer) {
            playerSpawnpoints.Add(child);
        }
        foreach (Transform child in pinSpawnpointsContainer) {
            pinSpawnpoints.Add(child);
        }
        foreach (Transform child in meteorSpawnpointsContainer) {
            meteorSpawnpoints.Add(child);
        }
    }

    private void Start() {
        foreach (SpawnCoins sc in spawnCoins) {
            StartCoroutine(CoinSpawnCoroutine(sc));
        }

        ToggleSpeech(false);
        foreach (Speech s in speech) {
            StartCoroutine(SpeechCoroutine(s));
        }

        foreach (float t in pinSpawnTimes) {
            StartCoroutine(SpawnPinCoroutine(t));
        }

        foreach (float t in meteorSpawnTimes) {
            StartCoroutine(SpawnMeteorCoroutine(t));
        }

        List<Transform> playerSpawns = new List<Transform>(playerSpawnpoints);
        for (int playerIndex = 0; playerIndex < GameInfo.GetMaxPlayers(); playerIndex++) {
            if (!GameInfo.GetPlayer(playerIndex)) continue;
            int randomSpawn = Random.Range(0, playerSpawns.Count);
            Player player = Instantiate(playerPrefab, playerSpawns[randomSpawn].position, playerSpawns[randomSpawn].rotation).GetComponent<Player>();
            player.Initialize(playerIndex, this);
            cameraManager.AddPlayerTransform(player);
            playerSpawns.RemoveAt(randomSpawn);
        }
    }

    private IEnumerator SpawnMeteorCoroutine(float delay) {
        yield return new WaitForSeconds(delay);
        Transform meteorSpawnpoint = meteorSpawnpoints[Random.Range(0, meteorSpawnpoints.Count)];
        Vector3 angles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        Meteor meteor = Instantiate(meteorPrefab, meteorSpawnpoint.position, Quaternion.Euler(angles)).GetComponent<Meteor>();
        meteor.Initialize(meteorSpawnpoint.forward);
    }

    private IEnumerator SpawnPinCoroutine(float delay) {
        yield return new WaitForSeconds(delay);
        Transform pinSpawnpoint = pinSpawnpoints[Random.Range(0, pinSpawnpoints.Count)];
        Instantiate(pinPrefab, pinSpawnpoint.position, pinSpawnpoint.rotation);
    }

    private IEnumerator SpeechCoroutine(Speech s) {
        yield return new WaitForSeconds(s.time);
        speechText.text = s.text;
        if (s.sound) {
            Sound.Play(s.sound, 1);
        }
        ToggleSpeech(true);
        yield return new WaitForSeconds(s.duration);
        ToggleSpeech(false);
    }

    private void ToggleSpeech(bool on) {
        foreach (GameObject g in enableWhenSpeech) {
            g.SetActive(on);
        }
    }

    private IEnumerator CoinSpawnCoroutine(SpawnCoins sc) {
        yield return new WaitForSeconds(sc.time);
        List<Transform> spawns = new List<Transform>(coinSpawnpoints);
        for (int i = 0; i < sc.amount; i++) {
            if (spawns.Count == 0) break;
            int spawnIndex = Random.Range(0, spawns.Count);
            Vector3 angles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            Instantiate(coinPrefab, spawns[spawnIndex].position, Quaternion.Euler(angles));
            spawns.RemoveAt(spawnIndex);
        }
    }

    private void Update() {
        if (TimeLeft > 0) {
            TimeLeft -= Time.deltaTime;
            TimeLeft = Mathf.Max(0, TimeLeft);
            OnTimeLeftChange?.Invoke(TimeLeft);
        } else {
            SceneManager.LoadScene("End");
        }
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            SceneManager.LoadScene("Start");
        }
    }

}