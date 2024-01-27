using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoins : MonoBehaviour {

    private Player player;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other) {
        Coin coin = other.GetComponent<Coin>();
        if (!coin) return;

        coin.PickUp();
        GameInfo.SetCoins(player.PlayerIndex, GameInfo.GetCoins(player.PlayerIndex) + 1);
    }

}