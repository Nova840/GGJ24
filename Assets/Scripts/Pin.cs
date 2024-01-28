using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour {

    [SerializeField]
    private float speed;

    private Rigidbody _rigidbody;
    private ObstacleCollider obstacleCollider;

    private GameObject follow;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        obstacleCollider = GetComponentInChildren<ObstacleCollider>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player")) return;
        _rigidbody.isKinematic = true;
        Destroy(obstacleCollider.gameObject);

        follow = new GameObject();
        follow.transform.SetPositionAndRotation(transform.position, transform.rotation);
        follow.transform.parent = collision.transform;
    }

    private void FixedUpdate() {
        if (!_rigidbody.isKinematic) {
            _rigidbody.velocity = transform.forward * speed;
        } else if (follow) {
            transform.SetPositionAndRotation(follow.transform.position, follow.transform.rotation);
        }
    }

    private void OnDestroy() {
        Destroy(follow);
    }

}