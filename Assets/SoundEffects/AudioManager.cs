using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<Sound> sounds;
    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.audioSource = s.source.AddComponent<AudioSource>();
            s.audioSource.clip = s.clip;

            s.audioSource.loop = s.loop;
            s.audioSource.volume = s.volume;
            s.audioSource.pitch = s.pitch;
            s.audioSource.panStereo = s.stereoPan;
            s.audioSource.spatialBlend = s.spacialBlend;
            s.audioSource.spread = s.spread;
            s.audioSource.rolloffMode = s.rolloffMode;
            s.audioSource.minDistance = s.minDistance;
            s.audioSource.maxDistance = s.maxDistance;
        }
    }

    public void PlaySound (string name)
    {
        Sound s = sounds.Find(Sound => Sound.name == name);
        s.audioSource.Play();
    }

    public void StopSound(string name)
    {
        Sound s = sounds.Find(Sound => Sound.name == name);
        s.audioSource.Stop();
    }

    public AudioSource GetAudioManager(string name)
    {
        Sound s = sounds.Find(Sound => Sound.name == name);
        return s.audioSource;
    }
}
