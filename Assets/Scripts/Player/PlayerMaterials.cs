using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaterials : MonoBehaviour {

    [SerializeField]
    private Renderer _renderer;

    [SerializeField]
    private Material[] playerMaterials;

    private Player player;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void Start() {
        _renderer.material = playerMaterials[player.PlayerIndex];
    }

}