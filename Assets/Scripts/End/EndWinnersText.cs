using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndWinnersText : MonoBehaviour {

    [SerializeField]
    private EndManager endManager;

    [SerializeField]
    private Color[] textColors;

    private TMP_Text text;

    private void Awake() {
        text = GetComponent<TMP_Text>();
    }

    private void Start() {
        string s = "";
        endManager.ForEachWinner(playerIndex => {
            s += $"<color=#{ColorUtility.ToHtmlStringRGBA(textColors[playerIndex])}>Player {playerIndex + 1}: {GameInfo.GetCoins(playerIndex)}</color>\n";
        });
        s = s.TrimEnd('\n');
        text.text = s;
    }

}