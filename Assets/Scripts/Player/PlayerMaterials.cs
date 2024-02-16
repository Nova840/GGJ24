using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaterials : MonoBehaviour {

    [SerializeField]
    private Material[] playerMaterials;

    [SerializeField]
    private Color[] playerOutlineColors;

    private Renderer _renderer;
    private Player player;
    private Outline outline;

    private void Awake() {
        _renderer = GetComponentInChildren<Renderer>();
        player = GetComponent<Player>();
        outline = GetComponent<Outline>();
    }

    private void Start() {
        _renderer.sharedMaterial = playerMaterials[player.PlayerIndex];
        outline.OutlineColor = playerOutlineColors[player.PlayerIndex];
    }

}