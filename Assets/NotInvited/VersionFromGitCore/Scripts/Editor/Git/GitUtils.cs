using NotInvited.VersionFromGit.Editor.Utils;
using NotInvited.VersionFromGit.Utils;
using System;
using System.Diagnostics;
using UnityEngine;

namespace NotInvited.VersionFromGit.Editor.Git
{
    /// <summary>
    /// Utils class to get information about git
    /// </summary>
    public static class GitUtils
    {
        /// <summary>
        /// Is git available for the current folder
        /// </summary>
        /// <returns></returns>
        public static bool IsGitAvailableForFolder()
        {
            string args = "status";

            var cmdResult = GitCmd(args, false);

            return cmdResult.success;
        }

        public static bool IsGitInstalled()
        {
            string args = "version";

            // Send cmd but don't log errors
            var cmdResult = GitCmd(args, false);

            return cmdResult.success;
        }

        /// <summary>
        /// Check if there's a version tag.
        /// If the result string is required use TryGetLastTag() instead
        /// </summary>
        /// <returns></returns>
        public static bool HasVersionTag()
        {

            var result = TryGetLastTag();

            return result.hasTag;
        }

        /// <summary>
        /// Return the last version tag. If no suitable tag is found it'll return the commit hash
        /// </summary>
        /// <returns></returns>
        public static (bool hasTag, string tag) TryGetLastTag(string tagFormat = "v[0-9]*")
        {
            string args = $@"describe --tags --long --always --match ""{tagFormat}*""";

            var cmdResult = GitCmd(args);

            // If no tag found it return the actual commit hash (7 char)
            bool tagFound = cmdResult.success && cmdResult.output.Length > 7;

            string tag = cmdResult.success == true ? cmdResult.output : "";

            return (tagFound, tag);
        }

        /// <summary>
        /// Get the name of the current commit branch
        /// </summary>
        /// <returns></returns>
        public static string GetBranchName()
        {
            string args = "rev-parse --abbrev-ref HEAD";

            var cmdResult = GitCmd(args);

            return cmdResult.output;
        }

        /// <summary>
        /// Get the date of the current commit
        /// </summary>
        /// <returns></returns>
        public static DateTime GetCommitDate()
        {
            string args = "log -1 --format=%ct";

            var cmdResult = GitCmd(args);

            if (int.TryParse(cmdResult.output, out int parseResult))
            {
                DateTime date = DateTimeUtils.GetDateTimeFromUnixTimestamp(parseResult);

                return date;
            }
            else
            {
                CustomLog.LogError($"Impossible to parse UnixTimestamp \"{cmdResult.output}\"");

                return new DateTime(0);
            }
        }

        /// <summary>
        /// Send Git commands and return results (output and error)
        /// </summary>
        /// <param name="arguments">Git command result</param>
        /// <param name="logError">Does it log error when cmd failed</param>
        /// <returns></returns>
        private static (bool success, string output, string error) GitCmd(string arguments, bool logError = true)
        {
            using (Process process = new Process())
            {
                int codeResult = process.Cmd("git",
                    arguments,
                    Application.dataPath,
                    out string output,
                    out string error);

                bool success = codeResult == 0;

                if (success == false && logError == true)
                {
                    CustomLog.LogError($"Error when cmd \"git {arguments}\" : {error}");
                }

                return (success, output, error);
            }
        }
    }
}
