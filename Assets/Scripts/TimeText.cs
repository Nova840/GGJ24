using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeText : MonoBehaviour {

    [SerializeField]
    private GameManager gameManager;

    private TMP_Text text;

    private void Awake() {
        text = GetComponent<TMP_Text>();
        gameManager.OnTimeLeftChange += OnTimeLeftChange;
    }

    private void OnTimeLeftChange(float timeLeft) {
        int secondsLeft = Mathf.CeilToInt(timeLeft);
        text.text = $"{secondsLeft / 60}:{(secondsLeft % 60).ToString("D2")}";
    }
}