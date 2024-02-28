using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrown : MonoBehaviour {

    [SerializeField]
    private GameObject crown;

    [SerializeField]
    private GameObject droppedCrownPrefab;

    [SerializeField, Range(0, 90)]
    private float crownLaunchAngleMin, crownLaunchAngleMax;

    [SerializeField]
    private float crownLaunchForce, crownLaunchTorque;

    [SerializeField]
    private float crownGrowTime;

    private Player player;

    private bool crownActive = false;

    private Coroutine growCrownCoroutine;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Start() {
        crown.SetActive(false);
        GameInfo.OnCoinsChange += OnCoinsChange;
    }

    private void OnDestroy() {
        GameInfo.OnCoinsChange -= OnCoinsChange;
    }

    private void OnCoinsChange(int playerIndex, int coins) {
        int maxCoins = 0;
        List<int> playersWithMaxCoins = new List<int>();
        for (int i = 0; i < GameInfo.GetNumPlayers(); i++) {
            int c = GameInfo.GetCoins(i);
            if (c != 0) {
                if (c > maxCoins) {
                    maxCoins = c;
                    playersWithMaxCoins.Clear();
                }
                if (c == maxCoins) {
                    playersWithMaxCoins.Add(i);
                }
            }
        }
        if (maxCoins == 0) {
            SetCrown(false);
        } else {
            SetCrown(playersWithMaxCoins.Contains(player.PlayerIndex));
        }
    }

    private void SetCrown(bool active) {
        if (!active && crownActive) {
            GameObject c = Instantiate(droppedCrownPrefab, crown.transform.position, crown.transform.rotation);
            Rigidbody rb = c.GetComponent<Rigidbody>();
            Vector3 launchDir = Vector3.RotateTowards(
                Vector3.up,
                Vector3.ProjectOnPlane(Random.onUnitSphere, Vector3.up),
                Random.Range(crownLaunchAngleMin, crownLaunchAngleMax) * Mathf.Deg2Rad,
                0
            );
            rb.AddForce(launchDir * crownLaunchForce, ForceMode.VelocityChange);
            rb.AddTorque(Random.onUnitSphere * crownLaunchTorque, ForceMode.VelocityChange);
            crown.SetActive(false);
        } else if (active && !crownActive) {
            GrowCrown();
            crown.SetActive(true);
        }
        crownActive = active;
    }

    private void GrowCrown() {
        if (growCrownCoroutine != null) {
            StopCoroutine(growCrownCoroutine);
        }
        growCrownCoroutine = StartCoroutine(Coroutine());
        IEnumerator Coroutine() {
            float scale = 0;
            while (scale < 1) {
                if (crownGrowTime > 0) {
                    scale += (1 / crownGrowTime) * Time.deltaTime;
                } else {
                    scale = 1;
                }
                crown.transform.localScale = Vector3.one * scale;
                yield return null;
            }
            growCrownCoroutine = null;
        }
    }

}