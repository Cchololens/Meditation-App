using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;

public class MeditationManager : MonoBehaviour
{

    public enum FadeType { In, Out };

    [Header("User Set")]
    [SerializeField] private bool day = true;
    [SerializeField] private UnityEngine.Rendering.Volume volume;


    [SerializeField] private float dayTransitionDuration = 10;
    [SerializeField] private float dayDelayDuration = 10;
    [SerializeField] private float whiteTransitionDuration = 30;
    [SerializeField] private float whiteDuration = 30;

    //[Range(0.0F, 1.0F)]
    //public float transitionPercent; // percent time takes for both ease-in and ease-out of meditation

    public ParticleSystem wind;
    //public ParticleSystem breatheIn;
    //public ParticleSystem breatheOut;

    [Header("Private Variables - Not Changeable")]
    [SerializeField] private float timeElapsed = 0;


    // Start is called before the first frame update
    void Start()
    {
        volume.weight = 0.0F;
        //totalDuration = easeInDuration + easeOutDuration;


        if (RenderSettings.skybox.GetFloat("_CubemapTransition") < .5)
        {
            day = true;
        }
        else
        {
            day = false;
        }

        StartCoroutine(SequenceStart());
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    //sequence of animations
    private IEnumerator SequenceStart()
    {
        yield return StartCoroutine(Fade(
            (result) => RenderSettings.skybox.SetFloat("_CubemapTransition", result),
            dayTransitionDuration,
            FadeType.In));

        yield return new WaitForSeconds(dayDelayDuration);

        yield return StartCoroutine(Fade( 
            (result) => volume.weight = result, 
            whiteTransitionDuration, 
            FadeType.In));

        yield return new WaitForSeconds(whiteDuration);

        yield return StartCoroutine(Fade(
            (result) => volume.weight = result,
            whiteTransitionDuration,
            FadeType.Out));

        yield return new WaitForSeconds(dayDelayDuration);

        yield return StartCoroutine(Fade(
            (result) => RenderSettings.skybox.SetFloat("_CubemapTransition", result),
            dayTransitionDuration,
            FadeType.Out));
    }

    //takes a float value and lerps it across a duration
    private IEnumerator Fade( Action<float> value, float transitionDuration, FadeType fadeType)
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
