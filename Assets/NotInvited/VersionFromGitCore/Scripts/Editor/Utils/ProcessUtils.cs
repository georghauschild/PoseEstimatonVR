using System.Diagnostics;
using System.Text;

namespace NotInvited.VersionFromGit.Editor.Utils
{
    public static class ProcessUtils
    {
        public static int Cmd(
            this Process process,
            string appName,
            string arguments,
            string workingDirectory,
            out string output,
            out string error)
        {
            process.StartInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                FileName = appName,
                Arguments = arguments,
                WorkingDirectory = workingDirectory
            };

            // Subscribe to event to get output and error
            var outputBuilder = new StringBuilder();
            var errorBuilder = new StringBuilder();
            process.OutputDataReceived += (_, args) => outputBuilder.AppendLine(args.Data);
            process.ErrorDataReceived += (_, args) => errorBuilder.AppendLine(args.Data);

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            process.WaitForExit();

            output = outputBuilder.ToString().TrimEnd();
            error = errorBuilder.ToString().TrimEnd();

            return process.ExitCode;
        }
    }
}
