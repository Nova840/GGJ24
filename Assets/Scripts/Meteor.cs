using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Meteor : MonoBehaviour {

    [SerializeField]
    private GameObject particlesOnHitPrefab;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float explosionRadius;

    [SerializeField]
    private float tilt = .5f;

    [SerializeField]
    private float move = 20;

    [SerializeField]
    private float percentCoinsLose = .5f;

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
        Player[] playersHit = Physics.OverlapSphere(transform.position, explosionRadius, LayerMask.GetMask("Player")).Select(c => c.GetComponent<Player>()).Where(r => r).ToArray();
        foreach (Player p in playersHit) {
            p.PlayerMove.ApplyHit(p.transform.position - transform.position, tilt, move);
            p.PlayerCoins.LoseCoinsByHit(percentCoinsLose);
        }
        Destroy(gameObject);
    }

}