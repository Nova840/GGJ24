using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoins : MonoBehaviour {

    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField]
    private Transform coinSpawnpoint;

    [SerializeField, Range(0, 90)]
    private float hitCoinLaunchMinDegrees, hitCoinLaunchMaxDegrees;

    [SerializeField, Range(0, 90)]
    private float fallCoinLaunchMinDegrees, fallCoinLaunchMaxDegrees;

    [SerializeField]
    private float hitCoinLaunchSpeed;

    [SerializeField]
    private float fallCoinLaunchSpeed;

    [SerializeField]
    private float coinLaunchInterval;

    [SerializeField]
    private float killYDepth;

    [SerializeField, Range(0, 1)]
    private float percentCoinsToLoseByFall;

    private CharacterActor characterActor;
    private Player player;

    private Vector3 coinPointLastGrounded;
    private Vector3 coinPointLastFell;

    private void Awake() {
        player = GetComponent<Player>();
        characterActor = GetComponent<CharacterActor>();
    }

    private void OnTriggerEnter(Collider other) {
        Coin coin = other.GetComponentInParent<Coin>();
        if (!coin) return;

        if (coin.CanPlayerPickUp(player.PlayerIndex)) {
            coin.PickUp();
            GameInfo.SetCoins(player.PlayerIndex, GameInfo.GetCoins(player.PlayerIndex) + 1);
        }
    }

    private void Update() {
        if (characterActor.IsGrounded) {
            coinPointLastGrounded = coinSpawnpoint.position;
        }
        if (transform.position.y <= killYDepth) {
            player.PlayerMove.SetRespawning();
            coinPointLastFell = coinSpawnpoint.position;
            LoseCoinsByFall(percentCoinsToLoseByFall);
        }
    }

    public void LoseCoinsByHit(float percentCoinsToLose) {
        StartCoroutine(LoseCoinsByHitCoroutine(percentCoinsToLose));
    }

    private IEnumerator LoseCoinsByHitCoroutine(float percentCoinsToLose) {
        int coinsToLose = CoinsToLose(percentCoinsToLose);
        for (int i = 0; i < coinsToLose; i++) {
            Coin coin = CreateCoin(coinSpawnpoint.position);

            Vector3 direction = GetDirection(hitCoinLaunchMinDegrees, hitCoinLaunchMaxDegrees);
            LaunchCoin(coin, direction, hitCoinLaunchSpeed);

            yield return new WaitForSeconds(coinLaunchInterval);
        }
    }

    private void LoseCoinsByFall(float percentCoinsToLose) {
        StartCoroutine(LoseCoinsByFallCoroutine(percentCoinsToLose));
    }

    private IEnumerator LoseCoinsByFallCoroutine(float percentCoinsToLose) {
        int coinsToLose = CoinsToLose(percentCoinsToLose);
        for (int i = 0; i < coinsToLose; i++) {
            Coin coin = CreateCoin(coinPointLastGrounded);

            Vector3 direction = GetDirection(fallCoinLaunchMinDegrees, fallCoinLaunchMaxDegrees);
            Vector3 fromGroundToPlayerXZ = coinPointLastFell - coinPointLastGrounded;
            fromGroundToPlayerXZ.y = 0;
            fromGroundToPlayerXZ = fromGroundToPlayerXZ.normalized;
            if (Vector3.Angle(fromGroundToPlayerXZ, direction) < 90) {
                direction = new Vector3(-direction.x, direction.y, -direction.z);//negate vector, then negate y again
            }
            LaunchCoin(coin, direction, fallCoinLaunchSpeed);

            yield return new WaitForSeconds(coinLaunchInterval);
        }
    }

    private void LaunchCoin(Coin coin, Vector3 direction, float coinLaunchSpeed) {
        coin.GetComponent<Rigidbody>().velocity = direction * coinLaunchSpeed;
    }

    private Vector3 GetDirection(float degreesMin, float degreesMax) {
        Vector2 randomInCircle = Random.insideUnitCircle.normalized;
        Vector3 randomInCircleXZ = new Vector3(randomInCircle.x, 0, randomInCircle.y);
        Vector3 direction = Vector3.Slerp(Vector3.up, randomInCircleXZ, Random.Range(degreesMin, degreesMax) / 90f);
        return direction;
    }

    private Coin CreateCoin(Vector3 spawnPosition) {
        Vector3 angles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        Coin coin = Instantiate(coinPrefab, spawnPosition, Quaternion.Euler(angles)).GetComponent<Coin>();
        coin.Initialize(player.PlayerIndex);
        return coin;
    }

    private int CoinsToLose(float percentCoinsToLose) {
        int coins = GameInfo.GetCoins(player.PlayerIndex);
        int coinsToLose = Mathf.CeilToInt(coins * percentCoinsToLose);
        GameInfo.SetCoins(player.PlayerIndex, coins - coinsToLose);
        return coinsToLose;
    }

}