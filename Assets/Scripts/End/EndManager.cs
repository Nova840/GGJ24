using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] endPlayers;

    [SerializeField]
    private Material[] endPlayerMaterials;

    private List<int> winners = new List<int>();//<playerIndex>

    public void ForEachWinner(Action<int> action) {
        foreach (int i in winners) {
            action?.Invoke(i);
        }
    }

    private void Awake() {
        for (int i = 0; i < GameInfo.GetMaxPlayers(); i++) {
            if (GameInfo.GetPlayer(i) != null) {
                winners.Add(i);
            }
        }
        winners = winners.OrderByDescending(playerIndex => GameInfo.GetCoins(playerIndex)).ToList();
    }

    private void Start() {
        for (int i = winners.Count; i < endPlayers.Length; i++) {
            endPlayers[i].SetActive(false);
        }

        for (int i = 0; i < winners.Count; i++) {
            endPlayers[i].GetComponentInChildren<Renderer>().material = endPlayerMaterials[winners[i]];
        }

        for (int i = 0; i < endPlayers.Length; i++) {
            if (!endPlayers[i].activeInHierarchy) continue;
            endPlayers[i].GetComponentInChildren<Animator>().Play("Stand", 0, (float)i / endPlayers.Length);
        }
    }

    private void Update() {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            SceneManager.LoadScene("Start");
        }
        for (int i = 0; i < Gamepad.all.Count; i++) {
            if (Gamepad.all[i].selectButton.wasPressedThisFrame) {
                SceneManager.LoadScene("Start");
            }
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame) {
            SceneManager.LoadScene("Game");
        }
        for (int i = 0; i < Gamepad.all.Count; i++) {
            if (Gamepad.all[i].startButton.wasPressedThisFrame) {
                SceneManager.LoadScene("Game");
            }
        }
    }

}