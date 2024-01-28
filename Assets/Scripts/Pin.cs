using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour {

    [SerializeField]
    private float speed;

    private Rigidbody _rigidbody;
    private Collider _collider;

    private GameObject follow;

    private Quaternion initialRotation;

    private bool stopped = false;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponentInChildren<Collider>();
    }

    private void Start() {
        initialRotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player")) return;
        stopped = true;
        _rigidbody.isKinematic = true;
        Destroy(_collider);

        follow = new GameObject();
        follow.transform.SetPositionAndRotation(transform.position, transform.rotation);
        follow.transform.parent = collision.transform;
    }

    private void FixedUpdate() {
        if (!_rigidbody.isKinematic) {
            transform.rotation = initialRotation;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.velocity = transform.forward * speed;
        } else if (follow) {
            transform.SetPositionAndRotation(follow.transform.position, follow.transform.rotation);
        }
    }

    private void OnDestroy() {
        Destroy(follow);
    }

}