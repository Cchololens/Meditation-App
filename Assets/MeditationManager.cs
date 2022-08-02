using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class MeditationManager : SharedFunction
{

    [Header("Game Objects")]
    [Tooltip("Particle effects for wind")]
    public ParticleSystem wind;
    [Tooltip("Particle effects for breathing in prompt")]
    public ParticleSystem breathIn;
    [Tooltip("Particle effects for breathing out prompt")]
    public ParticleSystem breathOut;
    private ParticleSystem.EmissionModule breathInEmission;
    private ParticleSystem.EmissionModule breathOutEmission;
    [Tooltip("Object for post processing effects")]
    [SerializeField] private UnityEngine.Rendering.Volume volume;
    [Tooltip("Trigger to start meditation")]
    public BubbleTrigger trigger;
    public AudioManager audioManager;

    [Header("User Set Variables (in seconds)")]
    [Tooltip("Time taken for day/night transition")]
    [SerializeField] private float dayTransitionDuration = 30;
    [Tooltip("Time delayed in between day/night transition and white transition")]
    [SerializeField] private float dayDelayDuration = 15;
    [Tooltip("Time taken to transition to fully white scene")]
    [SerializeField] private float whiteTransitionDuration = 30;
    [Tooltip("Time that the white effect and breathing prompts last")]
    [SerializeField] private float whiteDuration = 120;
    [Tooltip("Time between switching breathing in/out prompts")]
    [SerializeField] private float breathingIntervalDuration = 3;
    [Tooltip("Time that breathing particle effects stay on")]
    [SerializeField] private float breathingEmissionDuration = 1;
    [Tooltip("Time it takes before music fades in. Should ideally fade in somewhere during transition to white.")]
    [SerializeField] private float musicFadeDuration = 65;

    [Header("Exposed Variables - Not Changeable")]
    [SerializeField] private bool breathingIn = true;


    // Start is called before the first frame update
    void Start()
    {
        SetUpMeditation();
    }


    /*
     * Reset meditation space back to it's original state
     */
    private void SetUpMeditation()
    {
        volume.weight = 0.0F;
        RenderSettings.skybox.SetFloat("_CubemapTransition", 0.0F);

        breathInEmission = breathIn.emission;
        breathOutEmission = breathOut.emission;
        breathInEmission.enabled = false;
        breathOutEmission.enabled = false;


    }

    public void StartMeditation()
    {
        StartCoroutine(MeditationSequence());
    }


    /*
     * Sequence of events for the meditation.
     * Transitions skybox throuday/night, white transition, starts breathing prompts
     */
    private IEnumerator MeditationSequence()
    {
        SetUpMeditation();
      
        //sequence
        yield return StartCoroutine(Fade(
            (result) => RenderSettings.skybox.SetFloat("_CubemapTransition", result),
            dayTransitionDuration,
            FadeType.In));

        yield return new WaitForSeconds(dayDelayDuration);

        audioManager.Play("MeditationMusic");

        yield return StartCoroutine(Fade( 
            (result) => volume.weight = result, 
            whiteTransitionDuration, 
            FadeType.In));

        var breathingCoroutine = StartCoroutine(AlternateBreathing(whiteDuration));
        yield return new WaitForSeconds(whiteDuration);
        //clean up previous couroutine
        StopCoroutine(breathingCoroutine);
        breathInEmission.enabled = false;
        breathOutEmission.enabled = false;

        audioManager.FadeOut("MeditationMusic", musicFadeDuration);

        yield return StartCoroutine(Fade(
            (result) => volume.weight = result,
            whiteTransitionDuration,
            FadeType.Out));

        yield return new WaitForSeconds(dayDelayDuration);

        yield return StartCoroutine(Fade(
            (result) => RenderSettings.skybox.SetFloat("_CubemapTransition", result),
            dayTransitionDuration,
            FadeType.Out));

        trigger.OnEndMeditation(); 
    }
    

    /*
     * Turns particle effect streams on/off in loop to prompt when to breathe. 
     * Needs to be stopped and cleaned up outside coroutine.
     */
    private IEnumerator AlternateBreathing(float duration)
    {
        while (true)
        {
            if ((!breathingIn && breathInEmission.enabled) || (breathingIn && breathOutEmission.enabled)) //took breath
            {
                breathInEmission.enabled = false;
                breathOutEmission.enabled = false; //turn off breath
                yield return new WaitForSeconds(breathingIntervalDuration);
            }
            else if (breathingIn) //taking breath
            {
                breathInEmission.enabled = true;
                breathOutEmission.enabled = false; //turn on breath
                breathingIn = !breathingIn;
                yield return new WaitForSeconds(breathingEmissionDuration);
            }
            else
            {
                breathInEmission.enabled = false;
                breathOutEmission.enabled = true; //turn on breath
                breathingIn = !breathingIn;
                yield return new WaitForSeconds(breathingEmissionDuration);
            }
        }
       
    }



}
