using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;

[ExecuteInEditMode]
public class BubbleTrigger : MonoBehaviour
{

    public RectTransform fxHolder;
    public RectTransform fx;
    public Image circleFill;
    public float start;
    public float stop;

    [Range(0,1)]
    public float progress = 0f;
    bool meditationRunning = false;

    public GameObject group;
    public CanvasGroup canvasGroup;
    public MeditationManager manager;
    public AudioManager audioManager;
    public float fadeTime = 1;


    // Start is called before the first frame update
    void Start()
    {
        meditationRunning = false;
        circleFill.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (progress < 1 && !meditationRunning)
        {
            circleFill.fillAmount = progress;
            float newY = Mathf.Lerp(start, stop, progress);
            fx.localPosition = new Vector3(0, newY, 0);

        }else if(!meditationRunning)
        {
            meditationRunning = true;
            StartCoroutine(FadeOutTrigger(fadeTime));
        }

    }

    public IEnumerator FadeOutTrigger(float time)
    {
        audioManager.Play("TriggerStart");

        yield return manager.Fade(
            (result) => SetColor(result),
            time,
            MeditationManager.FadeType.Out);

        SetColor(0.0f);
        group.SetActive(false);

        yield return new WaitForSeconds(1);

        manager.StartMeditation();
    }

    public IEnumerator FadeInTrigger(float time)
    {
        progress = 0f;
        circleFill.fillAmount = progress;

        yield return new WaitForSeconds(1);

        group.SetActive(true);

        yield return manager.Fade(
           (result) => SetColor(result),
           time,
           MeditationManager.FadeType.In);
        
        audioManager.Play("TriggerEnd");
        yield return new WaitForSeconds(1);

        meditationRunning = false;

    }

    public void SetColor(float result)
    {
        canvasGroup.alpha = result;
    }

    public void OnEndMeditation()
    {
        StartCoroutine(FadeInTrigger(fadeTime));
    }

}
