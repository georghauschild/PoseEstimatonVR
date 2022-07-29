using NotInvited.VersionFromGit.Editor.Settings;
using UnityEngine;

namespace NotInvited.VersionFromGit.Editor.Utils
{
    public static class CustomLog
    {
        private static bool logEnable => VersionFromGitSettings.GetOrCreateSettings().AllowLogOnEditor;


        public static void Log(string message)
        {
            if (logEnable)
            {
                Debug.Log(message);
            }
        }

        public static void LogError(string message)
        {
            if (logEnable)
            {
                Debug.LogError(message);
            }
        }

        public static void LogWarning(string message)
        {
            if (logEnable)
            {
                Debug.LogWarning(message);
            }
        }
    }
}