using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    [SerializeField]
    private float samePlayerPickupDelay;

    [SerializeField]
    private float otherPlayerPickupDelay;

    [SerializeField]
    private AudioClip pickupClip;

    [SerializeField]
    private float bounceSoundVelocityThreshold;

    [SerializeField]
    private LayerMask layersToMakeBounceSound;

    [SerializeField]
    private AudioClip[] coinBounceClips;

    private int playerIndex = -1;//-1 means environment spawned it

    private float timeCreated;

    private void Start() {
        timeCreated = Time.time;
    }

    public void Initialize(int playerIndex) {
        this.playerIndex = playerIndex;
    }

    public void PickUp() {
        Sound.Play(pickupClip);
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
        Sound.Play(coinBounceClips[Random.Range(0, coinBounceClips.Length)]);
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