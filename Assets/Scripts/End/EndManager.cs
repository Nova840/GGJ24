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
            if (GameInfo.GetPlayer(i)) {
                winners.Add(i);
            }
        }
        winners = winners.OrderByDescending(playerIndex => GameInfo.GetCoins(playerIndex)).ToList();

        for (int i = 0; i < endPlayers.Length; i++) {
            if (!winners.Contains(i)) {
                endPlayers[i].SetActive(false);
            }
        }

        for (int i = 0; i < winners.Count; i++) {
            endPlayers[i].GetComponentInChildren<Renderer>().material = endPlayerMaterials[winners[i]];
        }
    }

    private void Update() {
        for (int i = 0; i < GameInfo.GetMaxPlayers(); i++) {
            if (Gamepad.all[0].selectButton.wasPressedThisFrame) {
                SceneManager.LoadScene("Start");
            }
        }
    }

}