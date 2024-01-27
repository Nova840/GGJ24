using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    [SerializeField]
    private float samePlayerPickupDelay;

    [SerializeField]
    private float otherPlayerPickupDelay;

    private int playerIndex = -1;//-1 means environment spawned it

    private float timeCreated;

    private void Start() {
        timeCreated = Time.time;
    }

    public void Initialize(int playerIndex) {
        this.playerIndex = playerIndex;
    }

    public void PickUp() {
        Destroy(gameObject);
    }

    public bool CanPlayerPickUp(int playerIndex) {
        float delay = playerIndex == this.playerIndex ? samePlayerPickupDelay : otherPlayerPickupDelay;
        if (Time.time - timeCreated < delay) {
            return false;
        } else {
            return true;
        }
    }

}