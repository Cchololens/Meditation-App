using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

namespace HP.VR.SpatialAudio
{
    public enum HPVRSpatialAudioEnableStatus : int
    {
        Not_Started = 0,
        In_Progress = 1,
        Enabled = 2,
        Failed = 3
    }

    public struct HPVRSpatialAudioEnableResult
    {
        public HPVRSpatialAudioEnableResult(HPVRSpatialAudioEnableStatus status = HPVRSpatialAudioEnableStatus.Not_Started, string statusMessage = "")
        {
            this.status = status;
            this.statusMessage = statusMessage;
        }
        public HPVRSpatialAudioEnableStatus status;
        public string statusMessage;
    }

    public static class HPVRSpatialAudioEnabler 
    {
        #region ImportedFunctions
        [DllImport("AudioPlugin-HP-VR-SpatialAudio", CallingConvention = CallingConvention.Cdecl)]
        private static extern int enableHPSpatialAudio(string clientId, string accessKey, int licenseModel);

        [DllImport("AudioPlugin-HP-VR-SpatialAudio", CallingConvention = CallingConvention.Cdecl)]
        private static extern int getHPSpatialAudioEnableStatus();

        [DllImport("AudioPlugin-HP-VR-SpatialAudio", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr getHPSpatialAudioEnableStatusMessage();
        #endregion

        static private HPVRSpatialAudioEnableStatus enableStatus = HPVRSpatialAudioEnableStatus.Not_Started;

        static public HPVRSpatialAudioEnableStatus EnableStatus
        {
            get
            {
                if (enableStatus != HPVRSpatialAudioEnableStatus.Enabled)
                {
                   enableStatus = (HPVRSpatialAudioEnableStatus)getHPSpatialAudioEnableStatus();
                }
                return enableStatus;
            }
        }

        public static void EnableSpatialAudio()
        {
            if (EnableStatus != HPVRSpatialAudioEnableStatus.Enabled)
            {
                HPVRSpatialAudioSettings settings = Resources.Load<HPVRSpatialAudioSettings>("HPVRSpatialAudioSettings");
                if (settings != null)
                {
                    enableStatus = (HPVRSpatialAudioEnableStatus)enableHPSpatialAudio(settings.ClientID, settings.AccessKey, settings.RequestedLicense);
                }
                else
                {
                    Debug.LogError("Could not find HP VR Spatial Audio Settings");
                }
            }
        }

        public static HPVRSpatialAudioEnableResult GetEnableResult()
        {
            var result = new HPVRSpatialAudioEnableResult(EnableStatus, "");
            if (result.status != HPVRSpatialAudioEnableStatus.In_Progress)
            {
                result.statusMessage = Marshal.PtrToStringAnsi(getHPSpatialAudioEnableStatusMessage());
            }
            return result;
        }

        public static bool IsEnabled()
        {
            return EnableStatus == HPVRSpatialAudioEnableStatus.Enabled;
        }
    }
}
