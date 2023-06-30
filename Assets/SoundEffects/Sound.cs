using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0, 1)]
    public float volume = 1;
    [Range(0.1f, 3)]
    public float pitch = 1;
    [Range(-1, 1)]
    public float stereoPan = 0;
    [Range(0, 1)]
    public float spacialBlend = 0;
    [Range(0,360)]
    public int spread = 0;
    public AudioRolloffMode rolloffMode;
    public int minDistance = 1;
    public int maxDistance = 500;

    public GameObject source;
    public AudioSource audioSource;


}
