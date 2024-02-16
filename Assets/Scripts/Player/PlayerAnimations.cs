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

    private Player player;
    private CharacterActor characterActor;
    private Animator animator;

    public bool Taunting { get; private set; } = false;

    private void Awake() {
        player = GetComponent<Player>();
        characterActor = GetComponent<CharacterActor>();
        animator = GetComponentInChildren<Animator>();
        player.PlayerPunch.OnAttack += OnAttack;
    }

    private void OnAttack() {
        if (Random.Range(0, 2) == 0) {
            animator.CrossFadeInFixedTime("Punch", punchBlendTime, 1);
        } else {
            animator.CrossFadeInFixedTime("Kick", kickBlendTime, 0);
        }
    }

    private void Update() {
        Vector3 velocityXZ = characterActor.Velocity;
        velocityXZ.y = 0;
        animator.SetFloat("Speed", velocityXZ.magnitude / player.PlayerMove.MoveSpeed);
        if (!Taunting && player.PlayerControls.GetTaunt()) {
            StartCoroutine(TauntCoroutine());
        }
    }

    private IEnumerator TauntCoroutine() {
        Taunting = true;
        animator.CrossFadeInFixedTime(tauntNames[Random.Range(0, tauntNames.Length)], tauntBlendTime, 0);
        animator.CrossFadeInFixedTime("Empty", tauntBlendTime, 1);
        yield return new WaitForSeconds(tauntTime);
        animator.CrossFadeInFixedTime("Movement", tauntBlendTime, 0);
        Taunting = false;
    }

}