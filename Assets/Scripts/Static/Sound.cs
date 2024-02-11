using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Sound {

    public static void Play(AudioClip clip, float volume = 1) {
        AudioSource sound = new GameObject("Sound: " + clip.name).AddComponent<AudioSource>();
        sound.clip = clip;
        sound.volume = volume;
        sound.Play();
        Object.Destroy(sound.gameObject, clip.length);
    }

}