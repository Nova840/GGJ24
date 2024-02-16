using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int PlayerIndex { get; private set; }
    public int PlayerInput {  get; private set; }
    public GameManager GameManager { get; private set; }

    public PlayerControls PlayerControls { get; private set; }
    public PlayerMove PlayerMove { get; private set; }
    public PlayerAttack PlayerAttack { get; private set; }
    public PlayerCoins PlayerCoins { get; private set; }
    public PlayerAnimations PlayerAnimations { get; private set; }

    private void Awake() {
        PlayerControls = GetComponent<PlayerControls>();
        PlayerMove = GetComponent<PlayerMove>();
        PlayerAttack = GetComponent<PlayerAttack>();
        PlayerCoins = GetComponent<PlayerCoins>();
        PlayerAnimations = GetComponent<PlayerAnimations>();
    }

    public void Initialize(int playerIndex, int playerInput, GameManager gameManager) {
        PlayerIndex = playerIndex;
        PlayerInput = playerInput;
        GameManager = gameManager;
    }

}