using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour {

    [SerializeField, Range(0, 3)]
    private int playerIndex;

    private TMP_Text text;

    private void Awake() {
        text = GetComponent<TMP_Text>();
        GameInfo.OnCoinsChange += OnCoinsChange;
    }

    private void OnDestroy() {
        GameInfo.OnCoinsChange -= OnCoinsChange;
    }

    private void Start() {
        UpdateText();
    }

    private void OnCoinsChange(int playerIndex, int coins) {
        if (this.playerIndex == playerIndex) {
            UpdateText();
        }
    }

    private void UpdateText() {
        if (GameInfo.GetPlayer(playerIndex) != null) {
            text.text = $"Player {playerIndex + 1}: {GameInfo.GetCoins(playerIndex)}";
        } else {
            text.text = "";
        }
    }

}