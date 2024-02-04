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

    [SerializeField]
    private float attackCooldown;

    private Player player;

    public event Action OnAttack;

    private float timeLastAttacked = Mathf.NegativeInfinity;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Update() {
        if (!player.PlayerMove.Respawning && player.PlayerControls.GetAttack()) {
            Attack();
        }
    }

    private void Attack() {
        if (Time.time - timeLastAttacked < attackCooldown) return;
        timeLastAttacked = Time.time;

        Sound.Play(attackClip, 1);
        Collider[] hitColliders = Physics.OverlapBox(
            attackTrigger.transform.position,
            attackTrigger.transform.lossyScale / 2f,
            attackTrigger.transform.rotation,
            LayerMask.GetMask("Player", "Attackable"),
            QueryTriggerInteraction.Collide
        );
        bool soundPlayed = false;
        foreach (Collider c in hitColliders) {
            if (c.gameObject.layer == LayerMask.NameToLayer("Player") && !c.isTrigger) {
                Player p = c.GetComponent<Player>();
                if (p && p == player) continue;
                if (!soundPlayed) {
                    Sound.Play(attackHitClip, 1);
                    soundPlayed = true;
                }
                p.PlayerMove.ApplyHit(p.transform.position - transform.position, attackTilt, attackMove);
                p.PlayerCoins.LoseCoinsByHit(percentCoinsToLose);
            } else if (c.gameObject.layer == LayerMask.NameToLayer("Attackable")) {
                c.GetComponent<SpawnOnAttack>().OnAttack();
            }
        }
        OnAttack?.Invoke();
    }

}