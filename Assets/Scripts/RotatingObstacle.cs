using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObstacle : MonoBehaviour {

    [SerializeField]
    private Vector3 rotateSpeed;

    private Vector3 positionLastFrame;
    private Quaternion rotationLastFrame;

    private Vector3 linearVelocity;
    private Vector3 angularVelocity;

    private void Update() {
        transform.Rotate(rotateSpeed * Time.deltaTime, Space.Self);
        linearVelocity = GetLinearVelocity();
        angularVelocity = GetAngularVelocity();
    }

    private void LateUpdate() {
        positionLastFrame = transform.position;
        rotationLastFrame = transform.rotation;
    }

    //Same as Rigidbody.GetPointVelocity
    public Vector3 GetPointVelocity(Vector3 position) {
        return linearVelocity + Vector3.Cross(angularVelocity, position - transform.position);
    }

    //https://forum.unity.com/threads/manually-calculate-angular-velocity-of-gameobject.289462/#post-3474071
    private Vector3 GetAngularVelocity() {
        Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(rotationLastFrame);

        deltaRotation.ToAngleAxis(out float angle, out Vector3 axis);

        angle *= Mathf.Deg2Rad;

        return (1f / Time.deltaTime) * angle * axis;
    }

    private Vector3 GetLinearVelocity() {
        return (transform.position - positionLastFrame) / Time.deltaTime;
    }


}