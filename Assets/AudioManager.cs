using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    public enum FadeType { In, Out };

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

    public IEnumerator Fade(Action<float> value, float transitionDuration, FadeType fadeType)
    {
        float timeElapsed = 0.0F;

        while (timeElapsed < transitionDuration)
        {
            if (fadeType == FadeType.In)
            {
                value(Mathf.Lerp(0.0F, 1.0F, timeElapsed / transitionDuration));
            }
            else
            {
                value(Mathf.Lerp(1.0F, 0.0F, timeElapsed / transitionDuration));
            }
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        //yield return new WaitForSeconds(transitionDuration);
    }

}
