using Lightbug.CharacterControllerPro.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour {

    [SerializeField]
    private float kickBlendInTime;

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
        animator.CrossFadeInFixedTime("Kick", kickBlendInTime, 0);
    }

    private void Update() {
        Vector3 XZSpeed = characterActor.Velocity;
        XZSpeed.y = 0;
        animator.SetFloat("Speed", XZSpeed.magnitude / player.PlayerMove.MoveSpeed);
    }

}