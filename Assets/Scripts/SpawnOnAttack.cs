using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnAttack : MonoBehaviour {

    [SerializeField]
    private GameObject spawnPrefab;

    [SerializeField]
    private float launchVelocityMin, launchVelocityMax;

    [SerializeField]
    private float launchAngleMin, launchAngleMax;

    [SerializeField]
    private float launchAngularVelocityMin, launchAngularVelocityMax;

    public void OnAttack() {
        Vector3 angles = new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));
        GameObject spawned = Instantiate(spawnPrefab, transform.position, Quaternion.Euler(angles));
        Rigidbody r = spawned.GetComponent<Rigidbody>();
        if (r) {
            Vector3 direction = Vector3.Slerp(transform.forward, Vector3.ProjectOnPlane(Random.onUnitSphere, transform.forward).normalized, Random.Range(launchAngleMin, launchAngleMax) / 90f).normalized;
            r.velocity = direction * Random.Range(launchVelocityMin, launchVelocityMax);
            r.angularVelocity = Random.onUnitSphere * Random.Range(launchAngularVelocityMin, launchAngularVelocityMax);
        }
    }

}