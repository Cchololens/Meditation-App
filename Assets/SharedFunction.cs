using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class SharedFunction : MonoBehaviour
{
    public enum FadeType { In, Out };

    //takes a float value and lerps it across a duration
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

    }
}
