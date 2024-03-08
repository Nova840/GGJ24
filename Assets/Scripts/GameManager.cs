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
    private GameObject ballPrefab;

    [SerializeField]
    private Transform coinSpawnpointsContainer;
    private List<Transform> coinSpawnpoints = new List<Transform>();

    [SerializeField]
    private float coinSpawnMaxRandomPosition;

    [SerializeField]
    private Transform playerSpawnpointsContainer;
    private List<Transform> playerSpawnpoints = new List<Transform>();
    public Transform GetRandomPlayerSpawnpoint() => playerSpawnpoints[Random.Range(0, playerSpawnpoints.Count)];

    [SerializeField]
    private float playerSpawnMaxRandomPosition;

    [SerializeField]
    private Transform pinSpawnpointsContainer;
    private List<Transform> pinSpawnpoints = new List<Transform>();

    [SerializeField]
    private float pinSpawnMaxRandomAngle;

    [SerializeField]
    private Transform ballSpawnpointsContainer;
    private List<Transform> ballSpawnpoints = new List<Transform>();

    [SerializeField]
    private float ballSpawnMaxRandomPosition;

    [SerializeField]
    private float totalTime;

    [SerializeField]
    private float coinSpawnAngularSpeed;

    public float TimeLeft { get; private set; }

    public event Action<float> OnTimeLeftChange;

    [SerializeField]
    private GameObject[] enableWhenSpeech;

    [SerializeField]
    private TMP_Text speechText;

    [Serializable]
    private class Speech {
        public float time;
        public float duration;
        public string text;
        public Sound sound;
    }

    [SerializeField]
    private Speech[] speech;

    [Serializable]
    private class Spawn {
        public float time;
        public int amount;
    }

    [SerializeField]
    private Spawn[] spawnCoins;

    [SerializeField]
    private Spawn[] spawnPins;

    [SerializeField]
    private Spawn[] spawnMeteors;

    private void Awake() {
        if (!GameInfo.StartSceneHasLoaded) {
            GameInfo.SetPlayer(0, -1);
        } else {
            for (int i = 0; i < GameInfo.GetMaxPlayers(); i++) {
                GameInfo.SetCoins(i, 0);
            }
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
        foreach (Transform child in ballSpawnpointsContainer) {
            ballSpawnpoints.Add(child);
        }
        SceneManager.LoadScene("Arena", LoadSceneMode.Additive);
    }

    private void Start() {
        ToggleSpeech(false);
        foreach (Speech s in speech) {
            StartCoroutine(SpeechCoroutine(s));
        }

        foreach (Spawn s in spawnCoins) {
            StartCoroutine(SpawnCoinCoroutine(s));
        }

        foreach (Spawn s in spawnPins) {
            StartCoroutine(SpawnPinCoroutine(s));
        }

        foreach (Spawn s in spawnMeteors) {
            StartCoroutine(SpawnBallCoroutine(s));
        }

        SpawnPlayers();
    }

    private void SpawnPlayers() {
        List<Transform> playerSpawns = new List<Transform>(playerSpawnpoints);
        for (int playerIndex = 0; playerIndex < GameInfo.GetMaxPlayers(); playerIndex++) {
            if (GameInfo.GetPlayer(playerIndex) == null) continue;
            int randomSpawn = Random.Range(0, playerSpawns.Count);
            Transform spawnpoint = playerSpawns[randomSpawn];
            Vector2 rDelta = Random.insideUnitCircle.normalized * playerSpawnMaxRandomPosition;
            Vector3 spawnPosition = spawnpoint.position + new Vector3(rDelta.x, 0, rDelta.y);
            Player player = Instantiate(playerPrefab, spawnPosition, spawnpoint.rotation).GetComponent<Player>();
            player.Initialize(playerIndex, (int)GameInfo.GetPlayer(playerIndex), this);
            cameraManager.AddPlayerTransform(player);
            playerSpawns.RemoveAt(randomSpawn);
        }
    }

    private IEnumerator SpawnBallCoroutine(Spawn spawn) {
        yield return new WaitForSeconds(spawn.time);
        List<Transform> spawns = new List<Transform>(ballSpawnpoints);
        for (int i = 0; i < spawn.amount; i++) {
            if (spawns.Count == 0) break;
            int spawnIndex = Random.Range(0, spawns.Count);
            Vector3 angles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            Transform spawnpoint = ballSpawnpoints[spawnIndex];
            Vector2 rDelta = Random.insideUnitCircle.normalized * ballSpawnMaxRandomPosition;
            Vector3 spawnPosition = spawnpoint.position + new Vector3(rDelta.x, 0, rDelta.y);
            Instantiate(ballPrefab, spawnPosition, Quaternion.Euler(angles)).GetComponent<Ball>();
            spawns.RemoveAt(spawnIndex);
        }
    }

    private IEnumerator SpawnPinCoroutine(Spawn spawn) {
        yield return new WaitForSeconds(spawn.time);
        List<Transform> spawns = new List<Transform>(pinSpawnpoints);
        for (int i = 0; i < spawn.amount; i++) {
            if (spawns.Count == 0) break;
            int spawnIndex = Random.Range(0, spawns.Count);
            Transform spawnpoint = pinSpawnpoints[spawnIndex];
            Quaternion spawnRotation = spawnpoint.rotation * Quaternion.AngleAxis(Random.Range(-pinSpawnMaxRandomAngle / 2f, pinSpawnMaxRandomAngle / 2f), Vector3.up);
            Instantiate(pinPrefab, spawnpoint.position, spawnRotation);
            spawns.RemoveAt(spawnIndex);
        }
    }

    private IEnumerator SpeechCoroutine(Speech s) {
        yield return new WaitForSeconds(s.time);
        speechText.text = s.text;
        if (s.sound.clip) {
            Sound.Play(s.sound);
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

    private IEnumerator SpawnCoinCoroutine(Spawn spawn) {
        yield return new WaitForSeconds(spawn.time);
        List<Transform> spawns = new List<Transform>(coinSpawnpoints);
        for (int i = 0; i < spawn.amount; i++) {
            if (spawns.Count == 0) break;
            int spawnIndex = Random.Range(0, spawns.Count);
            Vector3 angles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            Transform spawnpoint = spawns[spawnIndex];
            Vector2 rDelta = Random.insideUnitCircle.normalized * coinSpawnMaxRandomPosition;
            Vector3 spawnPosition = spawnpoint.position + new Vector3(rDelta.x, 0, rDelta.y);
            GameObject coin = Instantiate(coinPrefab, spawnPosition, Quaternion.Euler(angles));
            coin.GetComponent<Rigidbody>().angularVelocity = Random.onUnitSphere * coinSpawnAngularSpeed;
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