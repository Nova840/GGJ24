using Lightbug.CharacterControllerPro.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour {

    private Player player;
    private CharacterActor characterActor;

    private void Awake() {
        player = GetComponent<Player>();
        characterActor = GetComponent<CharacterActor>();
    }

    public Vector2 GetMove() {
        if (player.PlayerAnimations.Taunting) return Vector2.zero;
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
        if (player.PlayerAnimations.Taunting) return false;
        if (player.PlayerInput == -1) {
            return Keyboard.current.spaceKey.wasPressedThisFrame;
        } else if (player.PlayerInput == -2) {
            return Keyboard.current.rightCtrlKey.wasPressedThisFrame || Keyboard.current.rightShiftKey.wasPressedThisFrame;
        } else {
            return Gamepad.all[player.PlayerInput].buttonSouth.wasPressedThisFrame;
        }
    }

    public bool GetAttack() {
        if (player.PlayerAnimations.Taunting) return false;
        if (player.PlayerInput == -1) {
            return Keyboard.current.bKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame;
        } else if (player.PlayerInput == -2) {
            return Keyboard.current.periodKey.wasPressedThisFrame;
        } else {
            return Gamepad.all[player.PlayerInput].buttonWest.wasPressedThisFrame;
        }
    }

    public bool GetTaunt() {
        if (player.PlayerAnimations.Taunting) return false;
        if (!characterActor.IsGrounded) return false;
        if (player.PlayerInput == -1) {
            return Keyboard.current.nKey.wasPressedThisFrame;
        } else if (player.PlayerInput == -2) {
            return Keyboard.current.slashKey.wasPressedThisFrame;
        } else {
            return Gamepad.all[player.PlayerInput].buttonNorth.wasPressedThisFrame;
        }
    }

}