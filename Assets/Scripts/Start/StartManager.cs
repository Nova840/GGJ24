using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour {

    private void Start() {
        GameInfo.SetStartSceneHasLoaded();
    }

    private void Update() {
        for (int i = 0; i < GameInfo.GetNumPlayers(); i++) {
            if (Gamepad.all.Count <= i) return;
            if (Gamepad.all[i].buttonSouth.wasPressedThisFrame) {
                GameInfo.SetPlayer(i, true);
            }
            if (Gamepad.all[i].buttonEast.wasPressedThisFrame) {
                GameInfo.SetPlayer(i, false);
            }
            if (GameInfo.GetPlayer(i) && Gamepad.all[i].startButton.wasPressedThisFrame) {
                SceneManager.LoadScene("Game");
            }
        }
    }

}