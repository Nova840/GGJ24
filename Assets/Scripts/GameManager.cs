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
    private Transform coinSpawnpointsContainer;
    private List<Transform> coinSpawnpoints = new List<Transform>();

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
        TimeLeft = totalTime;
        foreach (Transform child in coinSpawnpointsContainer) {
            coinSpawnpoints.Add(child);
        }
    }

    private void Start() {
        foreach (SpawnCoins sc in spawnCoins) {
            StartCoroutine(CoinSpawnCoroutine(sc));
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