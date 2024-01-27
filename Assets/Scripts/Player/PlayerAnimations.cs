using Lightbug.CharacterControllerPro.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAnimations : MonoBehaviour {

    [SerializeField]
    private float kickBlendInTime;

    [SerializeField]
    private float punchBlendInTime;

    private Player player;
    private CharacterActor characterActor;
    private Animator animator;

    private void Awake() {
        player = GetComponent<Player>();
        characterActor = GetComponent<CharacterActor>();
        animator = GetComponentInChildren<Animator>();
        player.PlayerPunch.OnPunch += OnPunch;
    }

    private void OnPunch() {
        if (Random.Range(0, 2) == 0) {
            animator.CrossFadeInFixedTime("Punch", punchBlendInTime, 1);
        } else {
            animator.CrossFadeInFixedTime("Kick", kickBlendInTime, 0);
        }
    }

    private void Update() {
        Vector3 XZSpeed = characterActor.Velocity;
        XZSpeed.y = 0;
        animator.SetFloat("Speed", XZSpeed.magnitude / player.PlayerMove.MoveSpeed);
    }

}