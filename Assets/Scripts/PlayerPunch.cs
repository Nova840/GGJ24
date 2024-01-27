using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPunch : MonoBehaviour {

    [SerializeField]
    private BoxCollider punchTrigger;

    [SerializeField]
    private float punchTilt;

    [SerializeField]
    private float punchMove;

    private Player player;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Update() {
        if (Gamepad.all[player.PlayerIndex].buttonWest.wasPressedThisFrame) {
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
            }
        }
    }

}