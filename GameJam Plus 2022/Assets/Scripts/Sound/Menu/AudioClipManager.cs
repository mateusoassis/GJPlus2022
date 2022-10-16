using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.Audio;

public class AudioClipManager : MonoBehaviour
{
    [System.Serializable]
    public class SoundData
    {
        public string name;
        public AudioClip clip;
        public AudioSource source;
        [Range(0f, 1f)] public float volume;
        [Range(0.1f, 3f)] public float pitch;
        public bool loop;
    }
    public SoundData[] soundArray;

    private AudioSource audio;
    
    void Awake()
    {
        foreach(SoundData sound in soundArray)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    void LateUpdate()
    {
        foreach(SoundData sound in soundArray)
        {
            sound.source.volume = sound.volume;
        }
    }

    public void Play(string name)
    {
        SoundData sound = Array.Find(soundArray, soundArray => soundArray.name == name);
        sound.source.Play();
    }

    public void PlayOneShot(string name)
    {
        SoundData sound = Array.Find(soundArray, soundArray => soundArray.name == name);
        sound.source.PlayOneShot(sound.clip);
    }

    public void StopSound(string name)
    {
        SoundData sound = Array.Find(soundArray, soundArray => soundArray.name == name);
        sound.source.Stop();
    }
}
