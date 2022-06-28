using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MeditationManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.Rendering.Volume volume;
    [SerializeField] private float timeElapsed = 0;
   
    public float totalDuration;


    //public float transitionPercent;

    public float easeInDuration;
    public float easeOutDuration;

    // Start is called before the first frame update
    void Start()
    {
        volume.weight = 0.0F;
        totalDuration = easeInDuration + easeOutDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeElapsed < totalDuration)
        {
            if (timeElapsed < easeInDuration)
            {
                volume.weight = Mathf.Lerp(0.0F, 1.0F, timeElapsed / easeInDuration);
            }

            timeElapsed += Time.deltaTime;
        }
    }
}
