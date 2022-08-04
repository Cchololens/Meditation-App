using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

/*
 * Trigger that starts an event after you hold your gaze on the trigger object for a set period of time.
 */
[ExecuteInEditMode]
public class BubbleTrigger : SharedFunction
{
    [Header("Bubble Canvas Objects")]
    public RectTransform fxHolder;
    public RectTransform fx;
    public Image circleFill;
    public GameObject group;
    public CanvasGroup canvasGroup;

    [Header("Manager Objects")]
    public MeditationManager manager;
    public AudioManager audioManager;

    [Header("User Set Variables")]
    [Tooltip("Starting height of bubble's fill")]
    public float start;
    [Tooltip("Ending height of bubble's fill")]
    public float stop;
    [Tooltip("How much bubble is filled each tick you gaze at it.")]
    public float changeAmount = .005f;
    [Tooltip("Seconds it takes to fade bubble in/out")]
    public float fadeTime = 1f;
    [Tooltip("Scene to switch to. If null, starts meditation instead")]
    public string sceneName = null;

    [Header("Exposed Variables")]
    [Tooltip("Percent that bubble is filled") , Range(0,1)]
    public float progress = 0f;
    [Tooltip("If camera's raycast is hitting object")]
    public bool isRayHitting = false;
    [Tooltip("Coroutine to start an event is running")]
    bool triggerEventRunning = false;
    


    // Start is called before the first frame update
    void Start()
    {
        isRayHitting = false;
        triggerEventRunning = false;
        circleFill.fillAmount = 0f;
        progress = 0f;
    }

    void FixedUpdate()
    {
        if (isRayHitting && progress < 1f)
        {
            progress += changeAmount;
        }
        else if (!isRayHitting && progress > 0f)
        {
            progress -= changeAmount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (progress < 1 && !triggerEventRunning)
        {
            circleFill.fillAmount = progress;
            float newY = Mathf.Lerp(start, stop, progress);
            fx.localPosition = new Vector3(0, newY, 0);

        }
        else if (!triggerEventRunning)
        {
            triggerEventRunning = true;
            if ((sceneName == null || sceneName == "") && manager != null)
            {
                StartCoroutine(FadeOutTrigger(() => manager.StartMeditation(), fadeTime));
            }else if( sceneName != null)
            {
                StartCoroutine(FadeOutTrigger(() => SceneManager.LoadScene(sceneName) , fadeTime));
            }
        }

    }

    public IEnumerator FadeOutTrigger(Action functionToRun, float time)
    {
        audioManager.Play("TriggerStart");
        isRayHitting = false;

        yield return Fade(
            (result) => SetColor(result),
            time,
            FadeType.Out);

        SetColor(0.0f);
        group.SetActive(false);

        yield return new WaitForSeconds(1);

        functionToRun();
        
    }

    public IEnumerator FadeInTrigger(float time)
    {
        progress = 0f;
        circleFill.fillAmount = progress;

        yield return new WaitForSeconds(1);

        group.SetActive(true);

        yield return Fade(
           (result) => SetColor(result),
           time,
           FadeType.In);
        
        audioManager.Play("TriggerEnd");
        yield return new WaitForSeconds(1);

        triggerEventRunning = false;

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
