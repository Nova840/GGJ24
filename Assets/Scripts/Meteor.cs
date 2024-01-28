using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {

    [SerializeField]
    private GameObject particlesOnHitPrefab;

    [SerializeField]
    private float speed;

    private Rigidbody _rigidbody;

    private Vector3 spawnpointForward;

    public void Initialize(Vector3 spawnpointForward) {
        this.spawnpointForward = spawnpointForward;
    }

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        _rigidbody.velocity = spawnpointForward * speed;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.isTrigger || other.GetComponent<Meteor>()) return;
        if (particlesOnHitPrefab) {
            Instantiate(particlesOnHitPrefab, transform.position, particlesOnHitPrefab.transform.rotation);
        }
        Destroy(gameObject);
    }

}