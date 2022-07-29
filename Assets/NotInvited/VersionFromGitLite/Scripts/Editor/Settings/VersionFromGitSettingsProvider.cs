using NotInvited.VersionFromGit.Editor.Git;
using NotInvited.VersionFromGit.Editor.Settings;
using NotInvited.VersionFromGit.Editor.Utils;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NotInvited.VersionFromGit.Lite.Editor.Settings
{
    public static class VersionFromGitSettingsProvider
    {

        [SettingsProvider]
        public static SettingsProvider CreateMyCustomSettingsProvider()
        {
            var provider = new SettingsProvider("Project/VersionFromGitLite", SettingsScope.Project)
            {
                label = "Version From Git Lite",

                // Create the SettingsProvider and initialize its drawing (IMGUI) function in place:
                guiHandler = (searchContext) =>
                {
                    var settings = VersionFromGitSettings.GetSerializedSettings();
                    if (settings != null)
                    {
                        EditorGUI.indentLevel++;

                        GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
                        headerStyle.fontSize = 18;

                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("General", headerStyle);

                        EditorGUILayout.PropertyField(settings.FindProperty("AutomaticOnBuild"), new GUIContent("Automatic On Build"));
                        EditorGUILayout.PropertyField(settings.FindProperty("AllowLogOnEditor"), new GUIContent("Log enable"));

                        SettingsProviderUtils.DrawDefaultVersionPropertyField(settings.FindProperty("DefaultVersion"));

                        EditorGUILayout.Space();
                        EditorGUILayout.LabelField("Git", headerStyle);
                        SettingsProviderUtils.CheckIfGitIsInstalled(settings);

                        settings.ApplyModifiedProperties();

                        EditorGUI.indentLevel--;
                    }
                },

                keywords = new HashSet<string>(new[] { "Git", "Version", "Automatic", "Build" })
            };

            return provider;
        }
    }
}