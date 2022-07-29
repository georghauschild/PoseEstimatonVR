using System.IO;
using UnityEditor;
using UnityEngine;

namespace NotInvited.VersionFromGit.Editor.Settings
{
    /// <summary>
    /// Settings concerning the Version from Git tool
    /// </summary>
    public class VersionFromGitSettings : ScriptableObject
    {
        /// <summary>
        /// Name of the folder containing the settings
        /// </summary>
        public const string settingsFolderName = "Editor";

        /// <summary>
        /// Absolute path to the folder containing the settings
        /// </summary>
        public static string absoluteFolderPath => $"{Application.dataPath}/{settingsFolderName}";

        /// <summary>
        /// Path relative to Unity to the settings file
        /// </summary>
        public static string customSettingsPath => $"Assets/{settingsFolderName}/VersionFromGitSettings.asset";

        /// <summary>
        /// Is the version set automatically when game is build ?
        /// </summary>
        [HideInInspector]
        public bool AutomaticOnBuild = true;

        [HideInInspector]
        public bool AllowLogOnEditor = true;

        [HideInInspector]
        public string DefaultVersion = "0.1.0";

        [HideInInspector]
        public bool GitInstalled = false;

        [HideInInspector]
        public bool GitAvailable = false;

        [HideInInspector]
        public bool GitAvailableTested = false;

        [HideInInspector]
        public string VersionFormat = "{0}-{4}";

        [HideInInspector]
        public string DateFormat = "yyyyMMddHHmmss";

        public static string DefaultVersionFormat = "{0}-{4}";

        public static string DefaultDateFormat = "yyyyMMddHHmmss";

        /// <summary>
        /// Get or create the settings
        /// </summary>
        /// <returns>Version from git settings</returns>
        public static VersionFromGitSettings GetOrCreateSettings()
        {
            // Try to retrieve the settings
            var settings = AssetDatabase.LoadAssetAtPath<VersionFromGitSettings>(customSettingsPath);

            if (settings == null)
            {
                // Create the folder if it doesn't exist
                Directory.CreateDirectory(absoluteFolderPath);

                // Create new settings instance
                settings = CreateInstance<VersionFromGitSettings>();

                // Save the new settings
                AssetDatabase.CreateAsset(settings, customSettingsPath);
                AssetDatabase.SaveAssets();
            }

            return settings;
        }

        public static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }
}