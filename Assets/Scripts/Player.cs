using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int PlayerIndex { get; private set; }
    public PlayerMove PlayerMove { get; private set; }
    public PlayerPunch PlayerPunch { get; private set; }

    private void Awake() {
        PlayerMove = GetComponent<PlayerMove>();
        PlayerPunch = GetComponent<PlayerPunch>();
    }

    public void Initialize(int playerIndex) {
        PlayerIndex = playerIndex;
    }

}