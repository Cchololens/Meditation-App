using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/**
 * Manages and holds list of audio. 
 */
public class AudioManager : SharedFunction
{

    public Sound[] sounds;

    void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }


    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void FadeOut(string name, float duration)
    {
        StartCoroutine(FadeOutSequence(name, duration));
    }

    private IEnumerator FadeOutSequence(string name, float duration)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        yield return StartCoroutine(Fade(
          (result) => s.source.volume = result,
          duration,
          FadeType.Out));
        s.source.Stop();
        s.source.volume = s.volume;
    }

    public void FadeIn(string name, float duration)
    {
        StartCoroutine(FadeInSequence(name, duration));
    }

    private IEnumerator FadeInSequence(string name, float duration)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        yield return StartCoroutine(Fade(
          (result) => s.source.volume = result,
          duration,
          FadeType.In));
        s.source.Stop();
        s.source.volume = s.volume;
    }

}
