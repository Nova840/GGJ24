using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Ball : MonoBehaviour {

    [SerializeField]
    private GameObject particlesOnBouncePrefab, particlesOnDestroyPrefab;

    [SerializeField]
    private SphereCollider _collider;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private float speedLimit;

    [SerializeField]
    private float tilt = .5f;

    [SerializeField]
    private float move = 20;

    [SerializeField]
    private float percentCoinsLose = .5f;

    [SerializeField]
    private Sound hitGroundSound, hitGroundDestroySound;

    [SerializeField]
    private float minRotateSpeeed, maxRotateSpeeed;

    [SerializeField]
    private int bouncesToDestroy;

    [SerializeField]
    private float bounceHeight;

    [SerializeField]
    private float bounceRandomHorizontalVelocity;

    private Rigidbody _rigidbody;

    private int bounces;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start() {
        _rigidbody.angularVelocity = Random.onUnitSphere * Random.Range(minRotateSpeeed, maxRotateSpeeed);
    }

    private void FixedUpdate() {
        _rigidbody.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, speedLimit);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.isTrigger || other.GetComponent<Ball>()) return;

        bounces++;

        if (bounces < bouncesToDestroy) {
            Hit(particlesOnBouncePrefab, hitGroundSound, false);
            Vector3 bounceVelocity = Vector3.up * Mathf.Sqrt(2 * gravity * bounceHeight);
            Vector2 randomInsideCircle = Random.insideUnitCircle;
            bounceVelocity += new Vector3(randomInsideCircle.x, 0, randomInsideCircle.y) * bounceRandomHorizontalVelocity;
            _rigidbody.velocity = bounceVelocity;
        } else {
            Hit(particlesOnDestroyPrefab, hitGroundDestroySound, true);
        }
    }

    private void Hit(GameObject particlesPrefab, Sound sound, bool destroy) {
        if (particlesPrefab) {
            Instantiate(particlesPrefab, transform.position, particlesPrefab.transform.rotation);
        }
        float explosionRadius = _collider.transform.lossyScale.x * _collider.radius;
        bool didHit = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, explosionRadius, LayerMask.GetMask("Stadium"), QueryTriggerInteraction.Ignore);
        Vector3 overlapPosition = didHit ? hit.point : transform.position;
        Player[] playersHit = Physics.OverlapSphere(overlapPosition, explosionRadius, LayerMask.GetMask("Player")).Select(c => c.GetComponent<Player>()).Where(r => r).ToArray();
        foreach (Player p in playersHit) {
            p.PlayerMove.ApplyHit(p.transform.position - transform.position, tilt, move, percentCoinsLose);
        }
        Sound.Play(sound);
        if (destroy) {
            Destroy(gameObject);
        }
    }

}