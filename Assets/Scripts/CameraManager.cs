using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;

    private List<Player> players = new List<Player>();
    public void AddPlayerTransform(Player player) => players.Add(player);

    private void Update() {
        int count = players.Count;
        if (count == 0) return;

        Vector3 averagePos = Vector3.zero;
        foreach (Player player in players) {
            if (player.PlayerMove.Respawning) {
                count--;
                if (count == 0) return;
            } else {
                averagePos += player.transform.position;
            }
        }

        averagePos /= count;
        transform.position = Vector3.Lerp(transform.position, averagePos, moveSpeed * Time.deltaTime);
    }

}