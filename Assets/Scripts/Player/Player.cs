using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int PlayerIndex { get; private set; }
    public GameManager GameManager { get; private set; }

    public PlayerMove PlayerMove { get; private set; }
    public PlayerPunch PlayerPunch { get; private set; }
    public PlayerCoins PlayerCoins { get; private set; }
    public PlayerAnimations PlayerAnimations { get; private set; }

    private void Awake() {
        PlayerMove = GetComponent<PlayerMove>();
        PlayerPunch = GetComponent<PlayerPunch>();
        PlayerCoins = GetComponent<PlayerCoins>();
        PlayerAnimations = GetComponent<PlayerAnimations>();
    }

    public void Initialize(int playerIndex, GameManager gameManager) {
        PlayerIndex = playerIndex;
        GameManager = gameManager;
    }

}