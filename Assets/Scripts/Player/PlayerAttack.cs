using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    [SerializeField]
    private BoxCollider attackTrigger;

    [SerializeField, Range(0, 1)]
    private float attackTilt;

    [SerializeField]
    private float attackMove;

    [SerializeField, Range(0, 1)]
    private float percentCoinsToLose;

    [SerializeField]
    private AudioClip attackClip, attackHitClip;

    private Player player;

    public event Action OnAttack;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Update() {
        if (!player.PlayerMove.Respawning && player.PlayerControls.GetAttack()) {
            Attack();
        }
    }

    private void Attack() {
        Sound.Play(attackClip, 1);
        Collider[] playerColliders = Physics.OverlapBox(
            attackTrigger.transform.position,
            attackTrigger.transform.lossyScale / 2f,
            attackTrigger.transform.rotation,
            LayerMask.GetMask("Player")
        );
        bool soundPlayed = false;
        foreach (Collider c in playerColliders) {
            if (c.isTrigger) continue;
            Player p = c.GetComponent<Player>();
            if (p && p == player) continue;
            if (!soundPlayed) {
                Sound.Play(attackHitClip, 1);
                soundPlayed = true;
            }
            p.PlayerMove.ApplyHit(p.transform.position - transform.position, attackTilt, attackMove);
            p.PlayerCoins.LoseCoinsByHit(percentCoinsToLose);
        }
        OnAttack?.Invoke();
    }

}