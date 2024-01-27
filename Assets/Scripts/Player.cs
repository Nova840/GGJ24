using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour {

    [SerializeField]
    private Transform rotationPoint;

    [SerializeField]
    private float moveSpeed, getHitStrength;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private float jumpHeight;

    [SerializeField]
    private float acceleration;

    [SerializeField]
    private float hitRecovery;

    [SerializeField]
    private float rotatePointSmoothSpeed;

    private Camera mainCamera;
    private CharacterActor characterActor;

    private float currentYVelocity = 0;
    private Vector3 smoothMoveVectorXZ = Vector3.zero;
    private Vector3 hitMoveVelocityXZ = Vector3.zero;

    private Vector3 rotationPointUpDirection;

    private void Awake() {
        characterActor = GetComponent<CharacterActor>();
        mainCamera = Camera.main;
    }

    private void Update() {
        Move();
        if (Gamepad.current.buttonWest.wasPressedThisFrame) {
            ApplyRotation(Vector3.right, .5f, getHitStrength);
        }
    }

    private void LateUpdate() {
        rotationPointUpDirection = Vector3.Slerp(rotationPointUpDirection, Vector3.up, rotatePointSmoothSpeed * Time.deltaTime);
        rotationPoint.rotation = Quaternion.LookRotation(rotationPointUpDirection, transform.forward) * Quaternion.Euler(-90, 180, 0);
    }

    private void Move() {
        Vector2 inputVector = Gamepad.current.leftStick.value;
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        Vector3 moveVectorXZ = mainCamera.transform.TransformDirection(new Vector3(inputVector.x, 0, inputVector.y));
        moveVectorXZ.y = 0;
        moveVectorXZ = moveVectorXZ.normalized * inputVector.magnitude * moveSpeed;

        if (characterActor.IsGrounded) {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame) {
                currentYVelocity = Mathf.Sqrt(-jumpHeight * -2 * gravity);//formula to calculate velocity from jump height
            } else {
                currentYVelocity = -2;
            }
        } else {
            currentYVelocity -= gravity * Time.deltaTime;//multiplied by delta time twice on purpose
        }

        smoothMoveVectorXZ = Vector3.MoveTowards(smoothMoveVectorXZ, moveVectorXZ, acceleration * Time.deltaTime);
        hitMoveVelocityXZ = Vector3.MoveTowards(hitMoveVelocityXZ, Vector3.zero, hitRecovery * Time.deltaTime);

        Vector3 totalMoveXZ = smoothMoveVectorXZ + hitMoveVelocityXZ;

        characterActor.Velocity = new Vector3(totalMoveXZ.x, currentYVelocity, totalMoveXZ.z);

        if (smoothMoveVectorXZ != Vector3.zero) {
            characterActor.Rotation = Quaternion.LookRotation(smoothMoveVectorXZ, Vector3.up);
        }
    }

    public void ApplyRotation(Vector3 hitDirection, float rotateStrength, float moveStrength) {
        hitDirection.y = 0;
        hitDirection = hitDirection.normalized;
        if (hitDirection == Vector3.zero) return;
        Vector3 rotateUpDir = Vector3.Lerp(hitDirection, Vector3.up, rotateStrength);
        rotationPointUpDirection = rotateUpDir;
        rotationPoint.rotation = Quaternion.LookRotation(rotationPointUpDirection, transform.forward) * Quaternion.Euler(-90, 180, 0);
        hitMoveVelocityXZ += hitDirection * moveStrength;
    }

}
