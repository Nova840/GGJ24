using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoins : MonoBehaviour {

    private Player player;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void OnCollisionEnter(Collision collision) {
        Coin coin = collision.rigidbody.GetComponent<Coin>();
        if (!coin) return;

        coin.PickUp();
        GameInfo.SetCoins(player.PlayerIndex, GameInfo.GetCoins(player.PlayerIndex) + 1);
    }

}