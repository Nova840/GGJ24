using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour {

    private List<int> winners = new List<int>();//<playerIndex>
    public int GetNumWinners() => winners.Count;
    public int GetWinner(int index) => winners[index];
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

    private void Update() {
        CheckLoadScenes();
    }

    private void CheckLoadScenes() {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            SceneManager.LoadScene("Start");
            return;
        }
        for (int i = 0; i < Gamepad.all.Count; i++) {
            if (Gamepad.all[i].selectButton.wasPressedThisFrame) {
                SceneManager.LoadScene("Start");
                return;
            }
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame) {
            SceneManager.LoadScene("Game");
            return;
        }
        for (int i = 0; i < Gamepad.all.Count; i++) {
            if (Gamepad.all[i].startButton.wasPressedThisFrame) {
                SceneManager.LoadScene("Game");
                return;
            }
        }
    }
}