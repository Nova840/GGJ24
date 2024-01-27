using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo {

    private static bool[] players = { false, false, false, false };
    public static void SetPlayer(int index, bool value) {
        players[index] = value;
        OnPlayerChange?.Invoke(index, value);
    }
    public static bool GetPlayer(int index) => players[index];
    public static int GetNumPlayers() => players.Length;
    public static event Action<int, bool> OnPlayerChange;

}