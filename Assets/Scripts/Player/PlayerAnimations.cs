using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAnimations : MonoBehaviour {

    [SerializeField]
    private float kickBlendTime;

    [SerializeField]
    private float punchBlendTime;

    [SerializeField]
    private string[] tauntNames;

    [SerializeField]
    private float tauntTime;

    [SerializeField]
    private float tauntBlendTime;

    [SerializeField]
    private float standingJumpBlendTime;

    [SerializeField]
    private float runningJumpBlendTime;

    [SerializeField]
    private float fallBlendTime;

    [SerializeField]
    private float landingBlendTime;

    private Player player;
    private CharacterActor characterActor;
    private Animator animator;

    public bool Taunting { get; private set; } = false;

    private bool wasGroundedLastFrame = true;

    private void Awake() {
        player = GetComponent<Player>();
        characterActor = GetComponent<CharacterActor>();
        animator = GetComponentInChildren<Animator>();
        player.PlayerAttack.OnAttack += OnAttack;
        player.PlayerMove.OnJump += () => StartCoroutine(OnJumpCoroutine());
    }

    private IEnumerator OnJumpCoroutine() {
        if (PercentMaxRunSpeed() >= .5f) {
            animator.CrossFadeInFixedTime("Running Jump", runningJumpBlendTime, 0);
        } else {
            animator.CrossFadeInFixedTime("Standing Jump", standingJumpBlendTime, 0);
        }
        while (!characterActor.IsGrounded) {
            yield return null;
        }
        animator.CrossFadeInFixedTime("Movement", landingBlendTime, 0);
    }

    private void OnAttack() {
        if (Random.Range(0, 2) == 0) {
            animator.CrossFadeInFixedTime("Punch", punchBlendTime, 1);
        } else {
            animator.CrossFadeInFixedTime("Kick", kickBlendTime, 0);
        }
    }

    private void Update() {
        animator.SetFloat("Speed", PercentMaxRunSpeed());
        if (!player.PlayerControls.GetJump() && characterActor.IsGrounded != wasGroundedLastFrame) {
            if (characterActor.IsGrounded) {
                animator.CrossFadeInFixedTime("Movement", landingBlendTime, 0);
            } else {
                animator.CrossFadeInFixedTime("Falling", fallBlendTime, 0);
            }
        }
        if (!Taunting && player.PlayerControls.GetTaunt()) {
            StartCoroutine(TauntCoroutine());
        }
        wasGroundedLastFrame = characterActor.IsGrounded;
    }

    private IEnumerator TauntCoroutine() {
        Taunting = true;
        animator.CrossFadeInFixedTime(tauntNames[Random.Range(0, tauntNames.Length)], tauntBlendTime, 0);
        animator.CrossFadeInFixedTime("Empty", tauntBlendTime, 1);
        yield return new WaitForSeconds(tauntTime);
        animator.CrossFadeInFixedTime("Movement", tauntBlendTime, 0);
        Taunting = false;
    }

    public float PercentMaxRunSpeed() {
        Vector3 velocityXZ = characterActor.Velocity;
        velocityXZ.y = 0;
        return velocityXZ.magnitude / player.PlayerMove.MoveSpeed;
    }

}