using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCollider : MonoBehaviour {

    [SerializeField, Range(0, 1)]
    private float tilt = .5f;

    [SerializeField]
    private float move = 20;

    [SerializeField, Range(0, 1)]
    private float percentCoinsToLose = .1f;

    private Rigidbody _rigidbody;

    private void Awake() {
        _rigidbody = GetComponentInParent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        Player player = collision.collider.GetComponent<Player>();
        Vector3 direction = _rigidbody.GetPointVelocity(collision.GetContact(0).point);
        player.PlayerMove.ApplyHit(direction, tilt, move);
        player.PlayerCoins.LoseCoinsByHit(percentCoinsToLose);
    }

    //private void OnTriggerEnter(Collider other) {
    //    if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
    //    Player player = other.GetComponent<Player>();
    //    Vector3 direction = _rigidbody.GetPointVelocity(collision.GetContact(0).point);
    //    player.PlayerMove.ApplyHit(direction, tilt, move);
    //    player.PlayerCoins.LoseCoinsByHit(percentCoinsToLose);
    //}

}