using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo {

    public static bool StartSceneHasLoaded { get; private set; } = false;
    public static void SetStartSceneHasLoaded() => StartSceneHasLoaded = true;

    private static bool[] players = { false, false, false, false };
    public static void SetPlayer(int playerIndex, bool value) {
        players[playerIndex] = value;
        OnPlayerChange?.Invoke(playerIndex, value);
    }
    public static bool GetPlayer(int index) => players[index];
    public static int GetMaxPlayers() => players.Length;
    public static event Action<int, bool> OnPlayerChange;

    private static int[] coins = { 0, 0, 0, 0 };
    public static void SetCoins(int playerIndex, int coins) {
        GameInfo.coins[playerIndex] = coins;
        OnCoinsChange?.Invoke(playerIndex, coins);
    }
    public static int GetCoins(int playerIndex) => coins[playerIndex];
    public static event Action<int, int> OnCoinsChange;

}