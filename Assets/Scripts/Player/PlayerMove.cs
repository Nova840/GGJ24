using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    [SerializeField]
    private Transform rotationPoint;

    [SerializeField]
    private GameObject renderersContainer;

    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed => moveSpeed;

    [SerializeField]
    private float rotateSpeed;

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

    [SerializeField]
    private float respawnDelay;

    [SerializeField]
    private AudioClip[] deathScreams;

    private Player player;
    private Camera mainCamera;
    private CharacterActor characterActor;
    private Light spotlight;

    private float currentYVelocity = 0;
    private Vector3 smoothMoveVectorXZ = Vector3.zero;
    private Vector3 hitMoveVelocityXZ = Vector3.zero;

    private Vector3 rotationPointUpDirection;

    public bool Respawning { get; private set; }
    public void SetRespawning() => SetRespawning(true);
    private void SetRespawning(bool respawning) {
        Respawning = respawning;
        renderersContainer.SetActive(!respawning);
        characterActor.ColliderComponent.enabled = !respawning;
        spotlight.gameObject.SetActive(!respawning);
        if (respawning) {
            Sound.Play(deathScreams[Random.Range(0, deathScreams.Length)]);
            characterActor.Teleport(player.GameManager.GetRandomPlayerSpawnpoint());
            currentYVelocity = 0;
            smoothMoveVectorXZ = Vector3.zero;
            hitMoveVelocityXZ = Vector3.zero;
            StartCoroutine(SetRespawningCoroutine());
        }
    }
    private IEnumerator SetRespawningCoroutine() {
        yield return new WaitForSeconds(respawnDelay);
        SetRespawning(false);
    }

    private void Awake() {
        player = GetComponent<Player>();
        characterActor = GetComponent<CharacterActor>();
        mainCamera = Camera.main;
        spotlight = GetComponentInChildren<Light>();
    }

    private void Update() {
        if (!Respawning) {
            Move();
        }
    }

    private void LateUpdate() {
        rotationPointUpDirection = Vector3.Slerp(rotationPointUpDirection, Vector3.up, rotatePointSmoothSpeed * Time.deltaTime);
        rotationPoint.rotation = Quaternion.LookRotation(rotationPointUpDirection, transform.forward) * Quaternion.Euler(-90, 180, 0);
    }

    private void Move() {
        Vector2 inputVector = player.PlayerControls.GetMove();
        Vector3 mainCameraUpXZ = mainCamera.transform.up;
        if (mainCameraUpXZ == Vector3.up) {
            mainCameraUpXZ = mainCamera.transform.forward;
        }
        mainCameraUpXZ.y = 0;
        Quaternion mainCameraUpXZRotation = Quaternion.LookRotation(mainCameraUpXZ, Vector3.up);
        Vector3 moveVectorXZ = mainCameraUpXZRotation * new Vector3(inputVector.x, 0, inputVector.y);
        moveVectorXZ *= inputVector.magnitude * moveSpeed;

        if (characterActor.IsGrounded) {
            if (player.PlayerControls.GetJump()) {
                characterActor.ForceNotGrounded();
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

        if (moveVectorXZ != Vector3.zero) {
            characterActor.Rotation = Quaternion.Slerp(characterActor.Rotation, Quaternion.LookRotation(moveVectorXZ), rotateSpeed * Time.deltaTime);
        }
    }

    public void ApplyHit(Vector3 hitDirection, float rotateStrength, float moveStrength) {
        hitDirection.y = 0;
        hitDirection = hitDirection.normalized;
        if (hitDirection == Vector3.zero) return;
        Vector3 rotateUpDir = Vector3.Lerp(hitDirection, Vector3.up, rotateStrength);
        rotationPointUpDirection = rotateUpDir;
        rotationPoint.rotation = Quaternion.LookRotation(rotationPointUpDirection, transform.forward) * Quaternion.Euler(-90, 180, 0);
        hitMoveVelocityXZ += hitDirection * moveStrength;
    }

}
