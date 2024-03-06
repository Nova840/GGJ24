using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour {

    [SerializeField, Range(0, 3)]
    private int playerIndex;

    [SerializeField]
    private float spacing;

    private TMP_Text text;

    private void Awake() {
        text = GetComponentInChildren<TMP_Text>();
        GameInfo.OnCoinsChange += OnCoinsChange;
    }

    private void OnDestroy() {
        GameInfo.OnCoinsChange -= OnCoinsChange;
    }

    private void Start() {
        UpdateText();
        RectTransform rt = GetComponent<RectTransform>();
        float xPosition = (playerIndex - (GameInfo.GetMaxPlayers() - 1) / 2f) * spacing + (GameInfo.GetMaxPlayers() - GameInfo.GetNumPlayers()) * (spacing / 2f);
        rt.anchoredPosition = new Vector2(xPosition, rt.anchoredPosition.y);
    }

    private void OnCoinsChange(int playerIndex, int coins) {
        if (this.playerIndex == playerIndex) {
            UpdateText();
        }
    }

    private void UpdateText() {
        bool playerExists = GameInfo.GetPlayer(playerIndex) != null;
        if (playerExists) {
            text.text = $"Player {playerIndex + 1}: {GameInfo.GetCoins(playerIndex)}";
        } else {
            text.text = "";
        }
        if (gameObject.activeInHierarchy != playerExists) {
            gameObject.SetActive(playerExists);
        }
    }

}