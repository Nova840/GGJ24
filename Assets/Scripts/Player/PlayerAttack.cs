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
    private Sound missSound, hitSound;

    [SerializeField]
    private float attackCooldown;

    [SerializeField]
    private float attackPhysicsObjectForwardForce;

    [SerializeField]
    private float attackPhysicsObjectUpwardForce;

    [SerializeField]
    private float attackPhysicsObjectTorque;

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

        Vector3 halfExtents = new Vector3(
            attackTrigger.transform.lossyScale.x * attackTrigger.size.x / 2f,
            attackTrigger.transform.lossyScale.y * attackTrigger.size.z / 2f,
            attackTrigger.transform.lossyScale.z * attackTrigger.size.z / 2f
        );
        Collider[] hitColliders = Physics.OverlapBox(
            attackTrigger.transform.position,
            halfExtents,
            attackTrigger.transform.rotation,
            LayerMask.GetMask("Player", "Attackable", "Kitty"),
            QueryTriggerInteraction.Collide
        );
        bool hitSoundShouldPlay = false;
        foreach (Collider c in hitColliders) {
            if (c.gameObject.layer == LayerMask.NameToLayer("Player") && !c.isTrigger) {
                Player p = c.GetComponent<Player>();
                if (p && p == player) continue;
                p.PlayerMove.ApplyHit(p.transform.position - transform.position, attackTilt, attackMove, percentCoinsToLose);
                hitSoundShouldPlay = true;
            } else if (c.gameObject.layer == LayerMask.NameToLayer("Attackable")) {
                c.GetComponent<SpawnOnAttack>().OnAttack();
                hitSoundShouldPlay = true;
            } else if (c.gameObject.layer == LayerMask.NameToLayer("Kitty")) {
                Rigidbody r = c.GetComponentInParent<Rigidbody>();
                Vector3 force = attackTrigger.transform.forward * attackPhysicsObjectForwardForce + Vector3.up * attackPhysicsObjectUpwardForce;
                r.AddForce(force, ForceMode.VelocityChange);
                Vector3 torque = -Vector3.Cross(attackTrigger.transform.forward, Vector3.up) * attackPhysicsObjectTorque;
                r.AddTorque(torque, ForceMode.VelocityChange);
                hitSoundShouldPlay = true;
            }
        }
        Sound.Play(hitSoundShouldPlay ? hitSound : missSound);
        OnAttack?.Invoke();
    }

}