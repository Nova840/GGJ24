using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    [SerializeField]
    private float samePlayerPickupDelay;

    [SerializeField]
    private float otherPlayerPickupDelay;

    [SerializeField]
    private Sound pickupSound;

    [SerializeField]
    private float bounceSoundVelocityThreshold;

    [SerializeField]
    private LayerMask layersToMakeBounceSound;

    [SerializeField]
    private Sound[] coinBounceSounds;

    private int playerIndex = -1;//-1 means environment spawned it

    private float timeCreated;

    private bool destroying = false;

    private void Start() {
        timeCreated = Time.time;
    }

    public void Initialize(int playerIndex) {
        this.playerIndex = playerIndex;
    }

    public void PickUp() {
        destroying = true;
        Sound.Play(pickupSound);
        Destroy(gameObject);
    }

    private void Update() {
        if (transform.position.y <= -1000) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (!layersToMakeBounceSound.IsInMask(collision.gameObject.layer)) return;
        if (collision.relativeVelocity.magnitude < bounceSoundVelocityThreshold) return;
        Sound.Play(coinBounceSounds);
    }

    public bool CanPlayerPickUp(int playerIndex) {
        if (destroying) {
            return false;
        }
        float delay = playerIndex == this.playerIndex ? samePlayerPickupDelay : otherPlayerPickupDelay;
        if (Time.time - timeCreated < delay) {
            return false;
        } else {
            return true;
        }
    }

}