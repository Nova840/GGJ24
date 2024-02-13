using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour {

    private static List<Pin> instances = new List<Pin>();

    [SerializeField]
    private float speed;

    [SerializeField]
    private float destroyAfterIfNoHit;

    [SerializeField]
    private Sound pinStickSound;

    [SerializeField]
    private int maxAllowedPins;

    private Rigidbody _rigidbody;
    private Collider _collider;

    private GameObject follow;

    private Quaternion initialRotation;

    private void Awake() {
        instances.Add(this);
        while (instances.Count > maxAllowedPins) {
            Destroy(instances[0].gameObject);
            instances.RemoveAt(0);
        }
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponentInChildren<Collider>();
    }

    private IEnumerator Start() {
        initialRotation = transform.rotation;
        yield return new WaitForSeconds(destroyAfterIfNoHit);
        if (!_rigidbody.isKinematic) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player")) return;
        _rigidbody.isKinematic = true;
        Destroy(_collider);
        Sound.Play(pinStickSound);

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
        instances.Remove(this);
        Destroy(follow);
    }

}