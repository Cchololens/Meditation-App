using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class MeditationManager : MonoBehaviour
{

    public enum FadeType { In, Out };

    [Header("Game Objects")]
    public ParticleSystem wind;
    public ParticleSystem breathIn;
    public ParticleSystem breathOut;
    private ParticleSystem.EmissionModule breathInEmission;
    private ParticleSystem.EmissionModule breathOutEmission;
    [SerializeField] private UnityEngine.Rendering.Volume volume;
    public BubbleTrigger trigger;
    public AudioManager audioManager;

    [Header("User Set Variables")] //in seconds
    [SerializeField] private bool day = true;
    [SerializeField] private float dayTransitionDuration = 10;
    [SerializeField] private float dayDelayDuration = 10;
    [SerializeField] private float whiteTransitionDuration = 30;
    [SerializeField] private float whiteDuration = 30;
    [SerializeField] private float breathingIntervalDuration = 3;
    [SerializeField] private float breathingEmissionDuration = 1;
    [SerializeField] private float musicFadeDuration = 30;

    [Header("Private Variables - Not Changeable")]
    [SerializeField] private float timeElapsed = 0;
    [SerializeField] private bool breathingIn = true;


    // Start is called before the first frame update
    void Start()
    {
        SetUpMeditation();
    }

    private void SetUpMeditation()
    {
        //set-up
        volume.weight = 0.0F;
        RenderSettings.skybox.SetFloat("_CubemapTransition", 0.0F);

        breathInEmission = breathIn.emission;
        breathOutEmission = breathOut.emission;
        breathInEmission.enabled = false;
        breathOutEmission.enabled = false;

        if (RenderSettings.skybox.GetFloat("_CubemapTransition") < .5)
        {
            day = true;
        }
        else
        {
            day = false;
        }

    }


    public void StartMeditation()
    {
        StartCoroutine(MeditationSequence());
    }

    //sequence of animations
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


    //takes a float value and lerps it across a duration
    public IEnumerator Fade( Action<float> value, float transitionDuration, FadeType fadeType)
    {
            float timeElapsed = 0.0F;
            
            while (timeElapsed < transitionDuration)
            {
                if (fadeType == FadeType.In) {
                    value (Mathf.Lerp(0.0F, 1.0F, timeElapsed / transitionDuration));
                }
                else
                {
                    value ( Mathf.Lerp(1.0F, 0.0F, timeElapsed / transitionDuration));
                }
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            //yield return new WaitForSeconds(transitionDuration);
    }


    //needs to be stopped and cleaned up outside coroutine
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

        yield break;
       
    }



        /*
        private IEnumerator FadeIn(ParticleSystem ps, float transitionTime)
        {
            Debug.Log("aaa");
            float timer = 0.0F;

            //Material mat = pRenderer.material;

            while (timer < transitionTime)
            {
                var emission = ps.emission;
                //emission.rate = 0;
                emission.rateOverTime = 0;
                emission.rateOverDistance = 0;

                //var color = new Color(255f, 255f, 255f, 0f);
                //mat.color = color;
                timer++;
            }
            yield return new WaitForSeconds(transitionTime);
        }
        */

    }
