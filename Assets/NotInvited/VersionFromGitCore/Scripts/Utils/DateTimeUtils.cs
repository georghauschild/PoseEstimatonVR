using System;
namespace NotInvited.VersionFromGit.Utils
{
    public static class DateTimeUtils
    {
        public static DateTime GetDateTimeFromUnixTimestamp(double timestamp)
        {
            DateTime newDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            newDateTime = newDateTime.AddSeconds(timestamp).ToLocalTime();

            return newDateTime;
        }
    }
}