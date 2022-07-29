using NotInvited.VersionFromGit.Editor.Git;
using NotInvited.VersionFromGit.Editor.Settings;
using NotInvited.VersionFromGit.Editor.Utils;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace NotInvited.VersionFromGit.Editor
{
    public class AutomaticVersionOnBuild : IPreprocessBuildWithReport
    {
        public int callbackOrder { get; }

        /// <summary>
        /// Method called before building game
        /// </summary>
        /// <param name="report"></param>
        public void OnPreprocessBuild(BuildReport report)
        {
            if (IsAutomaticVersioningEnable() == false)
            {
                CustomLog.Log($"Automatic Version from git is disabled");

                // Automatic versioning is disabled -> exit
                return;
            }
            if (Git.GitUtils.IsGitAvailableForFolder() == false)
            {
                CustomLog.LogError("Git was not found on the project");
                return;
            }

            Debug.Log($"Automatic Version from git");

            string version = GetVersion();

            if (string.IsNullOrEmpty(version) == false)
            {
                PlayerSettings.bundleVersion = version;
                AssetDatabase.SaveAssets();

                CustomLog.Log($"Game version set to \"{PlayerSettings.bundleVersion}\"");
            }
            else
            {
                CustomLog.LogError("Game version not available");
            }
        }

        /// <summary>
        /// Get the version to apply
        /// </summary>
        /// <returns>Return null if nothing is specified</returns>
        private static string GetVersion()
        {
            string version = GitData.GetCurrentGitData().GetFormattedVersion();

            return version;
        }

        /// <summary>
        /// Check if the automatic versioning is enabled on Unity Settings
        /// </summary>
        /// <returns></returns>
        private static bool IsAutomaticVersioningEnable()
        {
            return VersionFromGitSettings.GetOrCreateSettings().AutomaticOnBuild;
        }
    }
}
