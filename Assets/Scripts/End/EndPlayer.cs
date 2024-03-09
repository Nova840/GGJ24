using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EndPlayer : MonoBehaviour {

    [SerializeField]
    private Renderer _renderer;

    [SerializeField]
    private EndManager endManager;

    [SerializeField]
    private GameObject crown;

    [SerializeField, Range(0, 3)]
    private int endPlayerIndex;

    [SerializeField]
    private Material[] endPlayerMaterials;

    [SerializeField]
    private float tauntDuration;

    [SerializeField]
    private float tauntTransitionDuration;

    private Animator animator;

    private bool taunting = false;
    private string animationName;

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
    }

    private void Start() {
        if (endPlayerIndex >= endManager.GetNumWinners()) {
            gameObject.SetActive(false);
            return;
        }

        int playerIndex = endManager.GetWinner(endPlayerIndex);
        _renderer.sharedMaterial = endPlayerMaterials[playerIndex];

        int coins = GameInfo.GetCoins(playerIndex);
        bool wearingCrown = coins == GameInfo.GetCoins(endManager.GetWinner(0)) && coins > 0;
        crown.SetActive(wearingCrown);

        float normalizedTime = Random.Range(0f, 1f);
        if (wearingCrown) {
            if (Random.Range(0, 2) == 0) {
                animationName = "Salsa";
            } else {
                animationName = "Laugh";
            }
            animator.Play(animationName, 0, normalizedTime);
        } else {
            if (gameObject.activeInHierarchy) {
                animationName = "Defeat";
                animator.Play(animationName, 0, normalizedTime);
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