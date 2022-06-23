using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HP.VR.SpatialAudio;


public class SAEnabler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HPVRSpatialAudioEnabler.EnableSpatialAudio();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
