using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerText : MonoBehaviour {

    [SerializeField, Range(0, 3)]
    private int playerIndex;

    private TMP_Text text;

    private void Awake() {
        text = GetComponent<TMP_Text>();
        GameInfo.OnPlayerChange += OnPlayerChange;
    }

    private void OnPlayerChange(int playerIndex, bool value) {
        if (playerIndex == this.playerIndex) {
            UpdateText();
        }
    }

    private void UpdateText() {
        text.text = GameInfo.GetPlayer(playerIndex) ? "Connected" : "Not Connected";
    }

}