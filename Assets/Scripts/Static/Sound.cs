using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

[Serializable]
public class Sound {

    public AudioClip clip;

    [Range(0, 1)]
    public float volume = 1;

    [Range(0, 1)]
    public float randomPitchVariation = 0;

    public static void Play(Sound[] sounds) => Play(sounds[Random.Range(0, sounds.Length)]);
    public static void Play(Sound sound) => Play(sound.clip, sound.volume, sound.randomPitchVariation);
    public static void Play(AudioClip clip, float volume = 1, float randomPitchVariation = 0) {
        AudioSource source = new GameObject("Sound: " + clip.name).AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = 1 + Random.Range(-randomPitchVariation, randomPitchVariation);
        source.Play();
        Object.Destroy(source.gameObject, clip.length * source.pitch);
    }

}