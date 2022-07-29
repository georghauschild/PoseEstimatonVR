using NotInvited.VersionFromGit.Editor.Git;
using NotInvited.VersionFromGit.Editor.Utils;
using System;
using UnityEditor;
using UnityEngine;

namespace NotInvited.VersionFromGit.Editor.Settings
{
    public static class SettingsProviderUtils
    {
        public static void DrawDefaultVersionPropertyField(SerializedProperty serializedProperty)
        {
            string initialValue = serializedProperty.stringValue;

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(serializedProperty, new GUIContent("Default Version"));
            EditorGUILayout.HelpBox("The Default Version is used when no version is founded.", MessageType.Info);

            if (EditorGUI.EndChangeCheck())
            {
                string newVersion = serializedProperty.stringValue;

                try
                {
                    serializedProperty.stringValue = new Version(newVersion).ToString();
                }
                catch
                {
                    serializedProperty.stringValue = initialValue;
                }
            }
        }

        private static GitData gitFullData;

        public static void CheckIfGitIsInstalled(SerializedObject settings)
        {
            var style = new GUIStyle(EditorStyles.helpBox);

            GUILayout.BeginVertical(style);

            bool isInstalled = settings.FindProperty("GitInstalled").boolValue;
            bool isAvailability = settings.FindProperty("GitAvailable").boolValue;

            GUIContent valid = EditorGUIUtility.IconContent("TestPassed");
            GUIContent failed = EditorGUIUtility.IconContent("TestFailed");
            GUIContent unknow = EditorGUIUtility.IconContent("TestNormal");

            EditorGUILayout.LabelField(new GUIContent(" Git installed", isInstalled ? valid.image : failed.image));
            EditorGUILayout.LabelField(new GUIContent(" Git on this project", isAvailability ? valid.image : failed.image));

            if (gitFullData != null)
            {
                EditorGUILayout.LabelField(new GUIContent(" Tag found", gitFullData.IsVersionTagFound ? valid.image : failed.image));

                EditorGUILayout.HelpBox(gitFullData.ToString(), MessageType.Info);
            }
            else
            {
                EditorGUILayout.LabelField(new GUIContent(" Tag found (Press \"Check Git last tag\" button)", unknow.image));
            }

            EditorGUILayout.Space();

            bool isTested = settings.FindProperty("GitAvailableTested").boolValue;

            if (GUILayout.Button("Check Git last tag") || isTested == false)
            {
                gitFullData = null;

                bool gitInstalled = GitUtils.IsGitInstalled();
                bool gitAvailable = false;

                if (gitInstalled)
                {
                    gitAvailable = GitUtils.IsGitAvailableForFolder();
                }

                settings.FindProperty("GitAvailableTested").boolValue = true;

                settings.FindProperty("GitInstalled").boolValue = gitInstalled;
                settings.FindProperty("GitAvailable").boolValue = gitAvailable;

                if (gitAvailable)
                {
                    CustomLog.Log("Git is installed and available for this project");

                    gitFullData = GitData.GetCurrentGitData();
                }
                else if (gitInstalled)
                {
                    CustomLog.Log("Git is installed but not initilized for this project");
                }
                else
                {
                    CustomLog.Log("Git is NOT installed or found. Check environment variable and git installation");
                }
            }

            GUILayout.EndVertical();
        }
    }
}