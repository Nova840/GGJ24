using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameInfo {

    public static bool StartSceneHasLoaded { get; private set; } = false;
    public static void SetStartSceneHasLoaded() => StartSceneHasLoaded = true;

    private static int?[] players = { null, null, null, null };
    public static void SetPlayer(int playerIndex, int? playerInput) {
        players[playerIndex] = playerInput;
        OnPlayerChange?.Invoke(playerIndex, playerInput);
    }
    public static bool PlayerWithInputExists(int playerInput) => players.Contains(playerInput);
    public static int? GetPlayer(int index) => players[index];
    public static int GetNumPlayers() => players.Count(p => p != null);
    public static int GetMaxPlayers() => players.Length;
    public static event Action<int, int?> OnPlayerChange;

    private static int[] coins = { 0, 0, 0, 0 };
    public static void SetCoins(int playerIndex, int coins) {
        GameInfo.coins[playerIndex] = coins;
        OnCoinsChange?.Invoke(playerIndex, coins);
    }
    public static int GetCoins(int playerIndex) => coins[playerIndex];
    public static event Action<int, int> OnCoinsChange;

}