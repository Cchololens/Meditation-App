using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
 * Starts the meditation sequence when hand enters a trigger object. 
 * Currently not used. Do not use if using other triggers to start meditation.
 */
public class MeditationTrigger : SharedFunction
{

    public Material mat;
    public Collider mCollider;
    public MeditationManager manager;
    public AudioManager audioManager;
    public float fadeTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mCollider = GetComponent<Collider>();
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Hand")
        {
            StartCoroutine(FadeOutTrigger(fadeTime));
        }
    }


    //called by manager once meditation ends, to re-establish sphere
    public void OnEndMeditation()
    {
        StartCoroutine(FadeInTrigger(fadeTime));
    }


    public IEnumerator FadeOutTrigger(float time)
    {
        audioManager.Play("TriggerStart");
        mCollider.enabled = false; //needs to be first to not trigger again

        yield return Fade(
            (result) => SetColor(result),
            time,
            FadeType.Out);

        SetColor(0.0f);

        yield return new WaitForSeconds(1);

        manager.StartMeditation();
    }

    public IEnumerator FadeInTrigger(float time)
    {
        yield return new WaitForSeconds(1);

        yield return Fade(
           (result) => SetColor(result),
           time,
           FadeType.In);

        mCollider.enabled = true;
        audioManager.Play("TriggerEnd");

    }

    public void SetColor(float result)
    {
        Color c = mat.color;
        c.a = result;
        mat.color = c;
    }
}
