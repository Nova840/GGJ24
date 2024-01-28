using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Sound {

    private static GameObject soundPrefab;

    public static void Play(AudioClip clip) {
        LoadSoundPrefabIfNone();
        AudioSource sound = Object.Instantiate(soundPrefab).GetComponent<AudioSource>();
        sound.clip = clip;
        sound.Play();
        Object.Destroy(soundPrefab, clip.length);
    }

    public static void LoadSoundPrefabIfNone() {
        if (!soundPrefab) {
            soundPrefab = Resources.Load<GameObject>("Sound");
        }
    }

}