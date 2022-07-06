using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeditationTrigger : MonoBehaviour
{

    public Material mat;
    public Collider collider;
    public MeditationManager manager;
    public AudioManager audioManager;
    public float fadeTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        collider.enabled = false; //needs to be first to not trigger again

        yield return manager.Fade(
            (result) => SetColor(result),
            time,
            MeditationManager.FadeType.Out);

        SetColor(0.0f);

        yield return new WaitForSeconds(1);

        manager.StartMeditation();
    }

    public IEnumerator FadeInTrigger(float time)
    {
        yield return new WaitForSeconds(1);

        yield return manager.Fade(
           (result) => SetColor(result),
           time,
           MeditationManager.FadeType.In);

        collider.enabled = true;
        audioManager.Play("TriggerEnd");

    }

    public void SetColor(float result)
    {
        Color c = mat.color;
        c.a = result;
        mat.color = c;
    }
}
