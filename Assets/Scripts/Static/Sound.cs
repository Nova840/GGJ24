using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class Sound {

    public static void Play(SoundParams s) => Play(s.clip, s.volume, s.randomPitchVariation);
    public static void Play(AudioClip clip, float volume = 1, float randomPitchVariation = 0) {
        AudioSource sound = new GameObject("Sound: " + clip.name).AddComponent<AudioSource>();
        sound.clip = clip;
        sound.volume = volume;
        sound.pitch = 1 + Random.Range(-randomPitchVariation, randomPitchVariation);
        sound.Play();
        Object.Destroy(sound.gameObject, clip.length * sound.pitch);
    }

}

[Serializable]
public class SoundParams {
    public AudioClip clip;
    [Range(0, 1)]
    public float volume = 1;
    [Range(0, 1)]
    public float randomPitchVariation = 0;
}