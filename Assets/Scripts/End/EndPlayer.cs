using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndPlayer : MonoBehaviour {

    [SerializeField]
    private EndManager endManager;

    [SerializeField, Range(0, 3)]
    private int endPlayerIndex;

    [SerializeField]
    private Material[] endPlayerMaterials;

    [SerializeField]
    private float tauntDuration;

    [SerializeField]
    private float tauntTransitionDuration;

    private Animator animator;
    private Renderer _renderer;

    private bool taunting = false;
    private string animationName;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
        _renderer = GetComponentInChildren<Renderer>();
    }

    private void Start() {
        if (endPlayerIndex >= endManager.GetNumWinners()) {
            gameObject.SetActive(false);
            return;
        }

        _renderer.sharedMaterial = endPlayerMaterials[endManager.GetWinner(endPlayerIndex)];

        if (endPlayerIndex == 0) {
            if (Random.Range(0, 2) == 0) {
                animationName = "Salsa";
            } else {
                animationName = "Laugh";
            }
            animator.Play(animationName, 0, 0);
        } else {
            if (gameObject.activeInHierarchy) {
                animationName = "Defeat";
                animator.Play(animationName, 0, (float)endPlayerIndex - 1 / endManager.GetNumWinners() - 1);
            }
        }
    }

    private void Update() {
        if (GetTaunt()) {
            StartCoroutine(TauntCoroutine());
        }
    }

    private IEnumerator TauntCoroutine() {
        taunting = true;
        animator.CrossFadeInFixedTime("Pose", tauntTransitionDuration);
        yield return new WaitForSeconds(tauntDuration);
        animator.CrossFadeInFixedTime(animationName, tauntTransitionDuration);
        taunting = false;
    }

    private bool GetTaunt() {
        if (taunting) return false;
        int playerIndex = endManager.GetWinner(endPlayerIndex);
        int playerInput = (int)GameInfo.GetPlayer(playerIndex);
        if (playerInput == -1) {
            return Keyboard.current.nKey.wasPressedThisFrame;
        } else if (playerInput == -2) {
            return Keyboard.current.slashKey.wasPressedThisFrame;
        } else {
            return Gamepad.all[playerInput].buttonNorth.wasPressedThisFrame;
        }
    }

}