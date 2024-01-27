using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoins : MonoBehaviour {

    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField]
    private Transform coinSpawnpoint;

    [SerializeField, Range(0, 90)]
    private float coinLaunchMinDegrees, coinLaunchMaxDegrees;

    [SerializeField]
    private float coinLaunchSpeed;

    [SerializeField]
    private float coinLaunchInterval;

    private Player player;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other) {
        Coin coin = other.GetComponentInParent<Coin>();
        if (!coin) return;

        if (coin.CanPlayerPickUp(player.PlayerIndex)) {
            coin.PickUp();
            GameInfo.SetCoins(player.PlayerIndex, GameInfo.GetCoins(player.PlayerIndex) + 1);
        }
    }

    public void LoseCoinsByHit(float percentCoinsToLose) {
        StartCoroutine(LoseCoinsByHitCoroutine(percentCoinsToLose));
    }

    private IEnumerator LoseCoinsByHitCoroutine(float percentCoinsToLose) {
        int coins = GameInfo.GetCoins(player.PlayerIndex);
        int coinsToLose = Mathf.CeilToInt(coins * percentCoinsToLose);
        GameInfo.SetCoins(player.PlayerIndex, coins - coinsToLose);
        for (int i = 0; i < coinsToLose; i++) {
            Vector3 angles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
            Coin coin = Instantiate(coinPrefab, coinSpawnpoint.position, Quaternion.Euler(angles)).GetComponent<Coin>();
            coin.Initialize(player.PlayerIndex);

            Vector2 randomInCircle = Random.insideUnitCircle.normalized;
            Vector3 randomInCircleXZ = new Vector3(randomInCircle.x, 0, randomInCircle.y);
            Vector3 direction = Vector3.Slerp(Vector3.up, randomInCircleXZ, Random.Range(coinLaunchMinDegrees, coinLaunchMaxDegrees) / 90f);
            coin.GetComponent<Rigidbody>().velocity = direction * coinLaunchSpeed;

            yield return new WaitForSeconds(coinLaunchInterval);
        }
    }

}