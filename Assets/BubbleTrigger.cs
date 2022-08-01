using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class BubbleTrigger : MonoBehaviour
{
    public enum FadeType { In, Out };
    public RectTransform fxHolder;
    public RectTransform fx;
    public Image circleFill;
    public float start;
    public float stop;

    [Range(0,1)]
    public float progress = 0f;
    public bool isRayHitting = false;
    public float changeAmount = .005f;
    public string sceneName = null;

    bool triggerEventRunning = false;
    public GameObject group;
    public CanvasGroup canvasGroup;
    public MeditationManager manager;
    public AudioManager audioManager;
    public float fadeTime = 1;


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
