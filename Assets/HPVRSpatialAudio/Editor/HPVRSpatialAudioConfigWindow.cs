// (c) Copyright 2020 HP Development Company, L.P.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HP.VR.SpatialAudio
{
    public class HPVRSpatialAudioConfigWindow : EditorWindow
    {
        protected HPVRSpatialAudioSettings m_settings;
        protected Editor m_settingsEditor;

        [MenuItem("HP VR Spatial Audio/Configure")]
        public static void CreateWindow()
        {
            HPVRSpatialAudioConfigWindow window = (HPVRSpatialAudioConfigWindow)EditorWindow.GetWindow(typeof(HPVRSpatialAudioConfigWindow), true);
            window.minSize = new Vector2(625.0f, 175.0f);
        }

        private void OnEnable()
        {
            titleContent = new GUIContent("HP VR Spatial Audio");
            m_settings = Resources.Load("HPVRSpatialAudioSettings") as HPVRSpatialAudioSettings;

            if(m_settings == null)
            {
                m_settings = CreateInstance<HPVRSpatialAudioSettings>();
                if (!AssetDatabase.IsValidFolder("Assets/Resources"))
                {
                    AssetDatabase.CreateFolder("Assets", "Resources");
                }
                AssetDatabase.CreateAsset(m_settings, "Assets/Resources/HPVRSpatialAudioSettings.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

        private void OnDisable()
        {
            if(m_settingsEditor != null)
            {
                DestroyImmediate(m_settingsEditor);
            }
        }

        void OnGUI()
        {
            if (m_settingsEditor == null)
            {
                m_settingsEditor = Editor.CreateEditor(m_settings);
            }
            m_settingsEditor.OnInspectorGUI();
        }
    }

    [CustomEditor(typeof(HPVRSpatialAudioSettings))]
    public class HPVRSpatialAudioSettingsEditor : Editor
    {
        string clientIDField = "";
        string accessKeyField = "";

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            HPVRSpatialAudioSettings spatialAudioSettings = (HPVRSpatialAudioSettings) target;

            clientIDField = EditorGUILayout.PasswordField(new GUIContent("Client ID", spatialAudioSettings.ClientID), spatialAudioSettings.ClientID);
            accessKeyField = EditorGUILayout.PasswordField(new GUIContent("Access Key", spatialAudioSettings.AccessKey), spatialAudioSettings.AccessKey);

            if (GUI.changed)
            {
                spatialAudioSettings.ClientID = clientIDField.Trim();
                spatialAudioSettings.AccessKey = accessKeyField.Trim();
                EditorUtility.SetDirty(target);
            }
        }
    }
}