using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPunch : MonoBehaviour {

    [SerializeField]
    private BoxCollider punchTrigger;

    [SerializeField, Range(0, 1)]
    private float punchTilt;

    [SerializeField]
    private float punchMove;

    [SerializeField, Range(0, 1)]
    private float percentCoinsToLose;

    private Player player;

    public event Action OnPunch;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Update() {
        if (Gamepad.all[player.PlayerIndex].buttonWest.wasPressedThisFrame) {
            Punch();
        }
    }

    private void Punch() {
        Collider[] playerColliders = Physics.OverlapBox(
            punchTrigger.transform.position,
            punchTrigger.transform.lossyScale / 2f,
            punchTrigger.transform.rotation,
            LayerMask.GetMask("Player")
        );
        foreach (Collider c in playerColliders) {
            if (c.isTrigger) continue;
            Player p = c.GetComponent<Player>();
            if (p && p == player) continue;
            p.PlayerMove.ApplyHit(p.transform.position - transform.position, punchTilt, punchMove);
            p.PlayerCoins.LoseCoinsByHit(percentCoinsToLose);
        }
        OnPunch?.Invoke();
    }
}