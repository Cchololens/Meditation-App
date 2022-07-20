// (c) Copyright 2019-2021 HP Development Company, L.P. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HP.Omnicept;
using HP.Omnicept.Messaging;
using HP.Omnicept.Messaging.Messages;
using System.IO;

public class DebugPrintMessages : MonoBehaviour
{
    [SerializeField]
    private int num = 1;

    [SerializeField]
    private bool showHeartRateMessages = true;
    [SerializeField]
    private bool showHeartRateVariabilityMessages = true;
    [SerializeField]
    private bool showPPGMessages = true;
    [SerializeField]
    private bool showCognitiveLoadMessages = true;

    [SerializeField]
    private bool showEyeTrackingMessages = true;

    [SerializeField]
    private bool showVsyncMessages = true;
    [SerializeField]
    private bool showCameraImageMessages = true;
    [SerializeField]
    private bool showCameraImageTexture = true;
    [SerializeField]
    private bool showIMUMessages = true;

    public Material cameraImageMat;
    private Texture2D cameraImageTex2D;

    private string clDocumentName;
    private string hrDocumentName;

    public void Start()
    {
        cameraImageTex2D = new Texture2D(400, 400, TextureFormat.R8, false);
        if (cameraImageMat != null)
        {
            cameraImageMat.mainTexture = cameraImageTex2D;
        }

        num = PlayerPrefs.GetInt("num");
        clDocumentName = Application.streamingAssetsPath + "/Data/" + "CognitiveLoad-" + num + ".csv";
        hrDocumentName = Application.streamingAssetsPath + "/Data/" + "HeartRate-" + num + ".csv";

        Directory.CreateDirectory(Application.streamingAssetsPath + "/Data/");
        //File.Delete(clDocumentName);
        if (!System.IO.File.Exists(clDocumentName))
        {
            File.WriteAllText(clDocumentName, "Time,CognitiveLoadValue,StandardDeviation,DataState\r\n");
        }
        //File.Delete(hrDocumentName);
        if (!System.IO.File.Exists(hrDocumentName))
        {
            File.WriteAllText(hrDocumentName, "Time,Rate\r\n");
        }
        PlayerPrefs.SetInt("num", num+1);
        PlayerPrefs.Save();

    }

    public void OnDestroy()
    {
        Destroy(cameraImageTex2D);
    }

    public void HeartRateHandler(HeartRate hr)
    {
        if (showHeartRateMessages && hr != null)
        {
            Debug.Log(hr);
            File.AppendAllText(hrDocumentName, System.DateTime.Now.ToString("hh:mm:ss") + "," + hr.Rate + "\r\n");
        }
    }

    public void HeartRateVariabilityHandler(HeartRateVariability hrv)
    {
        if (showHeartRateVariabilityMessages && hrv != null)
        {
            Debug.Log(hrv);
        }
    }

    public void PPGFrameHandler(PPGFrame ppg)
    {
        if (showPPGMessages && ppg != null)
        {
            Debug.Log(ppg);
        }
    }

    public void CognitiveLoadHandler(CognitiveLoad cl)
    {
        if (showCognitiveLoadMessages && cl != null)
        {
            Debug.Log(cl);
            File.AppendAllText(clDocumentName, System.DateTime.Now.ToString("hh:mm:ss") + "," + cl.CognitiveLoadValue + "," + cl.StandardDeviation + "," + cl.DataState + "\r\n");
        }
    }

    public void EyeTrackingHandler(EyeTracking eyeTracking)
    {
        if (showEyeTrackingMessages && eyeTracking != null)
        {
            Debug.Log(eyeTracking);
        }
    }

    public void VSyncHandler(VSync vsync)
    {
        if (showVsyncMessages && vsync != null)
        {
            Debug.Log(vsync);
        }
    }

    public void CameraImageHandler(CameraImage cameraImage)
    {
        if (cameraImage != null)
        {
            if (showCameraImageMessages)
            {
                Debug.Log(cameraImage);
            }
            if (showCameraImageTexture && cameraImageMat != null && cameraImage.SensorInfo.Location == "Mouth")
            {
                // Load data into the texture and upload it to the GPU.
                cameraImageTex2D.LoadRawTextureData(cameraImage.ImageData);
                cameraImageTex2D.Apply();
            }
        }
    }

    public void IMUFrameHandler(IMUFrame imu)
    {
        if (showIMUMessages && imu != null)
        {
            Debug.Log(imu);
        }
    }

    public void DisconnectHandler(string msg)
    {
        Debug.Log("Disconnected: " + msg);
    }

    public void ConnectionFailureHandler(HP.Omnicept.Errors.ClientHandshakeError error)
    {
        Debug.Log("Failed to connect: " + error);
    }
}
