using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour {

    private Player player;

    private void Awake() {
        player = GetComponent<Player>();
    }

    public Vector2 GetMove() {
        Vector2 inputVector;
        if (player.PlayerInput == -1) {
            inputVector = new Vector2(Keyboard.current.dKey.value - Keyboard.current.aKey.value, Keyboard.current.wKey.value - Keyboard.current.sKey.value);
        } else {
            inputVector = Gamepad.all[player.PlayerInput].leftStick.value;
        }
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        return inputVector;
    }

    public bool GetJump() {
        if (player.PlayerInput == -1) {
            return Keyboard.current.spaceKey.wasPressedThisFrame;
        } else {
            return Gamepad.all[player.PlayerInput].buttonSouth.wasPressedThisFrame;
        }
    }

    public bool GetAttack() {
        if (player.PlayerInput == -1) {
            return Mouse.current.leftButton.wasPressedThisFrame;
        } else {
            return Gamepad.all[player.PlayerInput].buttonWest.wasPressedThisFrame;
        }
    }

}