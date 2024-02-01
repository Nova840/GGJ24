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
        } else if (player.PlayerInput == -2) {
            inputVector = new Vector2(Keyboard.current.rightArrowKey.value - Keyboard.current.leftArrowKey.value, Keyboard.current.upArrowKey.value - Keyboard.current.downArrowKey.value);
        } else {
            inputVector = Gamepad.all[player.PlayerInput].leftStick.value;
        }
        inputVector = Vector2.ClampMagnitude(inputVector, 1);
        return inputVector;
    }

    public bool GetJump() {
        if (player.PlayerInput == -1) {
            return Keyboard.current.spaceKey.wasPressedThisFrame;
        } else if (player.PlayerInput == -2) {
            return Keyboard.current.rightCtrlKey.wasPressedThisFrame || Keyboard.current.rightShiftKey.wasPressedThisFrame;
        } else {
            return Gamepad.all[player.PlayerInput].buttonSouth.wasPressedThisFrame;
        }
    }

    public bool GetAttack() {
        if (player.PlayerInput == -1) {
            return Keyboard.current.bKey.wasPressedThisFrame;
        } else if (player.PlayerInput == -2) {
            return Keyboard.current.periodKey.wasPressedThisFrame;
        } else {
            return Gamepad.all[player.PlayerInput].buttonWest.wasPressedThisFrame;
        }
    }

}