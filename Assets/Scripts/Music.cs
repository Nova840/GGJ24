using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour {

    private static Music instance;

    private AudioSource audioSource;

    private void Awake() {
        if (instance) {
            Destroy(instance.gameObject);
        }
        instance = this;
        audioSource = GetComponent<AudioSource>();
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
        Destroy(gameObject, audioSource.clip.length);
    }

    private void OnDestroy() {
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    private void OnSceneChanged(Scene current, Scene next) {
        if (next.name == "Start") {
            Destroy(gameObject);
        }
    }

}