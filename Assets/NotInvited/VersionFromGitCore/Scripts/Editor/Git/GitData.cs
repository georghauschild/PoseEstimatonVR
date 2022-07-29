using NotInvited.VersionFromGit.Editor.Settings;
using NotInvited.VersionFromGit.Editor.Utils;
using System;
using System.Text;

namespace NotInvited.VersionFromGit.Editor.Git
{
    public class GitData
    {
        public string FullGitTagResult { get; private set; }

        public bool IsVersionTagFound { get; private set; }

        public bool IsGitAvailable { get; private set; }

        public int Major => Version.Major;
        public int Minor => Version.Minor;
        public int Revision => Version.Revision;

        public Version Version { get; private set; }

        public int NbCommitSinceTag { get; private set; }

        public string CommitHash { get; private set; }

        public DateTime CommitDate { get; private set; }

        public string Branch { get; private set; }


        private GitData()
        {
            FetchInformations();
        }

        private GitData(string fullTagExemple, DateTime commitDate, string branch)
        {
            FullGitTagResult = fullTagExemple;
            CommitDate = commitDate;
            Branch = branch;
            ParseDescribeResult();
        }

        /// <summary>
        /// Get version fromatted from arguments
        /// </summary>
        /// <param name="format"></param>
        /// <param name="dateFormat"></param>
        /// <returns></returns>
        public string GetFormattedVersion(string format, string dateFormat)
        {
            return string.Format(format,
                Version.ToString(),
                Major,
                Minor,
                Revision,
                CommitHash,
                CommitDate.ToString(dateFormat),
                Branch,
                NbCommitSinceTag,
                FullGitTagResult
                );
        }

        /// <summary>
        /// Get the version formatted by settings
        /// </summary>
        /// <param name="format"></param>
        /// <param name="dateFormat"></param>
        /// <returns></returns>
        public string GetFormattedVersion()
        {
            VersionFromGitSettings settings = VersionFromGitSettings.GetOrCreateSettings();
            return GetFormattedVersion(settings.VersionFormat, settings.DateFormat);
        }

        private void FetchInformations()
        {
            IsGitAvailable = GitUtils.IsGitAvailableForFolder();

            if (IsGitAvailable)
            {
                var result = GitUtils.TryGetLastTag();

                if(result.hasTag == false)
                {
                    result = GitUtils.TryGetLastTag("[0-9]*");
                }

                FullGitTagResult = result.tag;

                IsVersionTagFound = result.hasTag;

                CommitHash = FullGitTagResult;
                Version = new Version(VersionFromGitSettings.GetOrCreateSettings().DefaultVersion);

                try
                {
                    if (IsVersionTagFound)
                    {
                        ParseDescribeResult();
                    }
                }
                catch
                {

                }

                Branch = GitUtils.GetBranchName();
                CommitDate = GitUtils.GetCommitDate();
            }
        }

        private void ParseDescribeResult()
        {
            // Version
            string version = FullGitTagResult;

            // Find first index of the version indication
            int firstIndex = 0;

            char firstChar = version.ToCharArray()[0];
            if(firstChar == 'v' || firstChar == 'V')
            {
                firstIndex = 1;
            }
            // Get version string
            version = version.Substring(firstIndex, version.IndexOf('-') - firstIndex);
            Version = new Version(version);

            // Get the hash
            CommitHash = FullGitTagResult.Substring(FullGitTagResult.Length - 7, 7);

            // Number of commit since the last tag
            NbCommitSinceTag = ParseNbCommitSinceTag();
        }

        private int ParseNbCommitSinceTag()
        {
            int startIndex = FullGitTagResult.IndexOf('-') + 1;
            int endIndex = FullGitTagResult.LastIndexOf('-');
            int totalChar = endIndex - startIndex;
            string nbCommitSinceTagString = FullGitTagResult.Substring(startIndex, totalChar);

            if (int.TryParse(nbCommitSinceTagString, out int nbCommitSinceTag))
            {
                return nbCommitSinceTag;
            }
            else
            {
                CustomLog.LogError($"Failed to parse the nb commit since tag : \"{nbCommitSinceTagString}\"");
                return 0;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(nameof(FullGitTagResult)).Append(" : ").AppendLine(FullGitTagResult);
            sb.Append(nameof(IsVersionTagFound)).Append(" : ").AppendLine(IsVersionTagFound.ToString());
            sb.Append(nameof(IsGitAvailable)).Append(" : ").AppendLine(IsGitAvailable.ToString());
            sb.Append(nameof(Version)).Append(" : ").AppendLine(Version.ToString());
            sb.Append(nameof(NbCommitSinceTag)).Append(" : ").AppendLine(NbCommitSinceTag.ToString());
            sb.Append(nameof(CommitHash)).Append(" : ").AppendLine(CommitHash);
            sb.Append(nameof(CommitDate)).Append(" : ").AppendLine(CommitDate.ToString());
            sb.Append(nameof(Branch)).Append(" : ").Append(Branch);

            return sb.ToString();
        }

        public static bool IsFormattedVersionValid(string format, string dateFormat)
        {
            GitData example = GetExample();
            try
            {
                example.GetFormattedVersion(format, dateFormat);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static GitData GetExample()
        {
            GitData example = new GitData("v2.5.12-3-g7b860e2", new DateTime(2042, 1, 15, 15, 32, 25), "main");
            return example;
        }

        public static GitData GetCurrentGitData()
        {
            return new GitData();
        }
    }
}
