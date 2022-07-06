// (c) Copyright 2020 HP Development Company, L.P.


using UnityEngine;
using System.Collections;

namespace HP.VR.SpatialAudio
{
    [RequireComponent(typeof(AudioSource))]
    public class HPVRSpatializerUserParams : MonoBehaviour
    {
        [SerializeField]
        private bool shouldSpatialize = true;
        public bool ShouldSpatialize
        {
            get
            {
                return shouldSpatialize;
            }
            set
            {
                if (value != shouldSpatialize)
                {
                    shouldSpatialize = value;
                    UpdateAllSpatializerParams();
                }
            }
        }
        private bool isEnabled = false;

        [SerializeField]
        private float distanceAttn = 1.0f;
        public float DistanceAttn
        {
            get
            {
                return distanceAttn;
            }
            set
            {
                distanceAttn = value;
                source?.SetSpatializerFloat(0, distanceAttn);
                source?.GetSpatializerFloat(0, out distanceAttn);
            }
        }

        [SerializeField]
        private float fixedVolume = 0.0f;
        public float FixedVolume
        {
            get
            {
                return fixedVolume;
            }
            set
            {
                fixedVolume = value;
                source?.SetSpatializerFloat(1, fixedVolume);
                source?.GetSpatializerFloat(1, out fixedVolume);
            }
        }

        [SerializeField]
        private float customRolloff= 0.0f;
        public float CustomRolloff
        {
            get
            {
                return customRolloff;
            }
            set
            {
                customRolloff = value;
                source?.SetSpatializerFloat(2, customRolloff);
                source?.GetSpatializerFloat(2, out customRolloff);
            }
        }

        [SerializeField]
        private float hrtfAngleThreshold = 1.0f;
        public float HRTFAngleThreshold
        {
            get
            {
                return hrtfAngleThreshold;
            }
            set
            {
                hrtfAngleThreshold = value;
                source?.SetSpatializerFloat(3, hrtfAngleThreshold);
                source?.GetSpatializerFloat(3, out hrtfAngleThreshold);
            }
        }

        private void UpdateAllSpatializerParams()
        {
            if (source != null)
            {
                source.spatialize = isEnabled && ShouldSpatialize;
                DistanceAttn = distanceAttn;
                FixedVolume = fixedVolume;
                CustomRolloff = customRolloff;
                HRTFAngleThreshold = hrtfAngleThreshold;
            }
        }

        private AudioSource source = null;

        void Start()
        {
            isEnabled = HPVRSpatialAudioEnabler.IsEnabled();
            source = GetComponent<AudioSource>();
            if (source != null)
            {
                UpdateAllSpatializerParams();
            }
            else
            {
                Debug.LogError("Audio source is required to use HP VR Spatial Audio.");
            }
        }

        void Update()
        {
            if (ShouldSpatialize)
            {
                if (source != null)
                {
                    if (!isEnabled && HPVRSpatialAudioEnabler.IsEnabled())
                    {
                        isEnabled = HPVRSpatialAudioEnabler.IsEnabled();
                        UpdateAllSpatializerParams();
                    }
                }
                else
                {
                    source = GetComponent<AudioSource>();
                    if(source != null)
                    {
                        UpdateAllSpatializerParams();
                    }
                }
            }
        }
    }
}
