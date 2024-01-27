using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private float totalTime;

    public float TimeLeft { get; private set; }

    public event Action<float> OnTimeLeftChange;

    private void Awake() {
        TimeLeft = totalTime;
    }

    private void Update() {
        if (TimeLeft > 0) {
            TimeLeft -= Time.deltaTime;
            TimeLeft = Mathf.Max(0, TimeLeft);
            OnTimeLeftChange?.Invoke(TimeLeft);
        } else {
            SceneManager.LoadScene("End");
        }
    }

}