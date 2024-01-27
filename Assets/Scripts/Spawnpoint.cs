using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour {

    [SerializeField]
    private GameObject playerPrefab;

    [SerializeField, Range(0, 3)]
    private int playerIndex;

    private void Start() {
        if (GameInfo.GetPlayer(playerIndex) || (!GameInfo.StartSceneHasLoaded && playerIndex == 0)) {
            Player player = Instantiate(playerPrefab, transform.position, transform.rotation).GetComponent<Player>();
            player.Initialize(playerIndex);
        }
    }

}