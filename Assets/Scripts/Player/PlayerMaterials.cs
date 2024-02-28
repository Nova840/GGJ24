using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaterials : MonoBehaviour {

    [SerializeField]
    private Renderer _renderer;

    [SerializeField]
    private Material[] playerMaterials;

    [SerializeField]
    private Color[] playerOutlineColors;

    private Player player;
    private Outline outline;

    private void Awake() {
        player = GetComponent<Player>();
        outline = GetComponent<Outline>();
    }

    private void Start() {
        Material[] materials = _renderer.sharedMaterials;
        materials[0] = playerMaterials[player.PlayerIndex];
        _renderer.sharedMaterials = materials;
        outline.OutlineColor = playerOutlineColors[player.PlayerIndex];
    }

}