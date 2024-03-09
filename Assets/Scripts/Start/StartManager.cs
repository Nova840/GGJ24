using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartManager : MonoBehaviour {

    private void Awake() {
        GameInfo.SetStartSceneHasLoaded();
        SceneManager.LoadScene("Arena", LoadSceneMode.Additive);
    }

    private void Update() {
        CheckQuitGame();

        CheckAddPlayers();

        CheckRemovePlayers();

        CheckLoadGame();
    }

    private void CheckAddPlayers() {
        if (Keyboard.current.spaceKey.wasPressedThisFrame && !GameInfo.PlayerWithInputExists(-1)) {
            GameInfo.SetPlayer(GameInfo.GetNumPlayers(), -1);
        }
        if ((Keyboard.current.rightCtrlKey.wasPressedThisFrame || Keyboard.current.rightShiftKey.wasPressedThisFrame) && !GameInfo.PlayerWithInputExists(-2)) {
            GameInfo.SetPlayer(GameInfo.GetNumPlayers(), -2);
        }
        for (int playerInput = 0; playerInput < Gamepad.all.Count; playerInput++) {
            if (Gamepad.all[playerInput].buttonSouth.wasPressedThisFrame && !GameInfo.PlayerWithInputExists(playerInput)) {
                GameInfo.SetPlayer(GameInfo.GetNumPlayers(), playerInput);
            }
        }
    }

    private void CheckRemovePlayers() {
        for (int playerIndex = 0; playerIndex < GameInfo.GetNumPlayers(); playerIndex++) {
            int playerInput = (int)GameInfo.GetPlayer(playerIndex);
            if (playerInput == -1) {
                if (Keyboard.current.bKey.wasPressedThisFrame) {
                    ClearPlayer(playerIndex);
                }
            } else if (playerInput == -2) {
                if (Keyboard.current.periodKey.wasPressedThisFrame) {
                    ClearPlayer(playerIndex);
                }
            } else {
                if (Gamepad.all[playerInput].buttonEast.wasPressedThisFrame) {
                    ClearPlayer(playerIndex);
                }
            }
        }
    }

    private void CheckLoadGame() {
        for (int playerIndex = 0; playerIndex < GameInfo.GetNumPlayers(); playerIndex++) {
            int playerInput = (int)GameInfo.GetPlayer(playerIndex);
            if (playerInput == -1) {
                if (Keyboard.current.enterKey.wasPressedThisFrame) {
                    SceneManager.LoadScene("Game");
                    return;
                }
            } else if (playerInput == -2) {
                if (Keyboard.current.enterKey.wasPressedThisFrame) {
                    SceneManager.LoadScene("Game");
                    return;
                }
            } else {
                if (Gamepad.all[playerInput].startButton.wasPressedThisFrame) {
                    SceneManager.LoadScene("Game");
                    return;
                }
            }
        }
    }

    private void ClearPlayer(int playerIndex) {
        GameInfo.SetPlayer(playerIndex, null);
        for (int i = playerIndex; i < GameInfo.GetMaxPlayers(); i++) {
            if (i == GameInfo.GetMaxPlayers() - 1) {
                GameInfo.SetPlayer(i, null);
            } else {
                GameInfo.SetPlayer(i, GameInfo.GetPlayer(i + 1));
            }
        }
    }

    private void CheckQuitGame() {
        if (Keyboard.current.escapeKey.wasPressedThisFrame) {
            QuitGame();
        }
        for (int i = 0; i < Gamepad.all.Count; i++) {
            if (Gamepad.all[i].selectButton.wasPressedThisFrame) {
                QuitGame();
            }
        }
    }

    private void QuitGame() {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}