using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartPlayer : MonoBehaviour {

    [SerializeField, Range(0, 3)]
    private int playerIndex;

    private TMP_Text text;

    private void Awake() {
        text = GetComponentInChildren<TMP_Text>();
        GameInfo.OnPlayerChange += OnPlayerChange;
    }

    private void OnDestroy() {
        GameInfo.OnPlayerChange -= OnPlayerChange;
    }

    private void Start() {
        UpdateText();
    }

    private void OnPlayerChange(int playerIndex, int? playerInput) {
        UpdateText();
    }

    private void UpdateText() {
        if (GameInfo.GetPlayer(playerIndex) != null) {
            SetText("Ready");
        } else {
            if (GameInfo.GetNumPlayers() == playerIndex) {
                if (GameInfo.PlayerWithInputExists(-1)) {
                    SetText("Press A to join");
                } else {
                    SetText("Press A/SPACE to join");
                }
            } else {
                SetText("");
            }
        }
    }

    private void SetText(string text) {
        this.text.text = text;
        this.text.gameObject.SetActive(this.text.text != "");
    }

}