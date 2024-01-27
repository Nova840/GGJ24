using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour {

    private Player player;
    private CharacterActor characterActor;
    private Animator animator;

    private void Awake() {
        player = GetComponent<Player>();
        characterActor = GetComponent<CharacterActor>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update() {
        Vector3 XZSpeed = characterActor.Velocity;
        XZSpeed.y = 0;
        animator.SetFloat("Speed", XZSpeed.magnitude / player.PlayerMove.MoveSpeed);
    }

}