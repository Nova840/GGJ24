using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObstacleCollider : MonoBehaviour {

    [SerializeField, Range(0, 1)]
    private float tilt = .5f;

    [SerializeField]
    private float move = 20;

    [SerializeField, Range(0, 1)]
    private float percentCoinsToLose = .1f;

    [SerializeField]
    private Collider[] obstacleColliders;

    private Rigidbody _rigidbody;
    private RotatingObstacle rotatingObstacle;

    private void Awake() {
        _rigidbody = GetComponentInParent<Rigidbody>();
        rotatingObstacle = GetComponent<RotatingObstacle>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        if (!obstacleColliders.Contains(collision.GetContact(0).thisCollider)) return;
        Player player = collision.collider.GetComponent<Player>();
        Vector3 direction;
        if (rotatingObstacle) {
            direction = rotatingObstacle.GetPointVelocity(collision.GetContact(0).point);
        } else {
            direction = _rigidbody.GetPointVelocity(collision.GetContact(0).point);
        }
        player.PlayerMove.ApplyHit(direction, tilt, move, percentCoinsToLose);
    }

}