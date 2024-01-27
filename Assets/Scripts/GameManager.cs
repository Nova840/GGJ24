using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField]
    private Transform coinSpawnpointsContainer;
    private List<Transform> coinSpawnpoints = new List<Transform>();

    [SerializeField]
    private Transform playerSpawnpointsContainer;
    private List<Transform> playerSpawnpoints = new List<Transform>();
    public Transform GetRandomPlayerSpawnpoint() => playerSpawnpoints[Random.Range(0, playerSpawnpoints.Count)];

    [SerializeField]
    private float totalTime;

    public float TimeLeft { get; private set; }

    public event Action<float> OnTimeLeftChange;

    [Serializable]
    private class SpawnCoins {
        public float time;
        public float amount;
    }

    [SerializeField]
    private SpawnCoins[] spawnCoins;

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
    }

    private void Start() {
        foreach (SpawnCoins sc in spawnCoins) {
            StartCoroutine(CoinSpawnCoroutine(sc));
        }

        List<Transform> playerSpawns = new List<Transform>(playerSpawnpoints);
        for (int playerIndex = 0; playerIndex < GameInfo.GetMaxPlayers(); playerIndex++) {
            if (!GameInfo.GetPlayer(playerIndex)) continue;
            int randomSpawn = Random.Range(0, playerSpawns.Count);
            Player player = Instantiate(playerPrefab, playerSpawns[randomSpawn].position, playerSpawns[randomSpawn].rotation).GetComponent<Player>();
            player.Initialize(playerIndex, this);
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
    }

}