namespace HireMe.Firewall.Checker.Core
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using HireMe.Firewall.Checker.Core.Enum;
    using HireMe.Firewall.Checker.Core.Interface;
    using Microsoft.Extensions.Configuration;

    public class WindowsDefenderScanner: IScanner
    {
        private readonly IConfiguration _config;

        public WindowsDefenderScanner(IConfiguration config)
        {
            _config = config;
        }

        public ScanResult Scan(string file, int timeoutInMs = 30000)
        {
            if (!File.Exists(file))
            {
                return ScanResult.FileNotFound;
            }

            string path = _config.GetSection("MySettings").GetSection("WindowsDefenderPath").Value;
            var full = new FileInfo(path).FullName;
            var fileInfo = new FileInfo(file);

            var process = new Process();

            var startInfo = new ProcessStartInfo(full)
            {
                Arguments = $"-Scan -ScanType 3 -File \"{fileInfo.FullName}\" -DisableRemediation",
                CreateNoWindow = true,
                ErrorDialog = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false
            };

            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit(timeoutInMs);

            if (!process.HasExited)
            {
                process.Kill();
                return ScanResult.Timeout;
            }

            switch (process.ExitCode)
            {
                case 0:
                    return ScanResult.NoThreatFound;
                case 2:
                    return ScanResult.ThreatFound;
                default:
                    return ScanResult.Error;
            }
        }
        
        public Task<int> ScanAsync(string fileName, int timeoutInMs = 30000)
        {
            // File to scan
            string path = _config.GetSection("MySettings").GetSection("WindowsDefenderPath").Value;
            var full = new FileInfo(path).FullName;
            var fileInfo = new FileInfo(fileName);

            var tcs = new TaskCompletionSource<int>();

            var process = new Process
            {
                StartInfo = 
                {
                FileName = full,
                Arguments = $"-Scan -ScanType 3 -File \"{fileInfo.FullName}\" -DisableRemediation",
                CreateNoWindow = true,
                ErrorDialog = false,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false
                },

                EnableRaisingEvents = true
            };

            process.Exited += (sender, args) =>
            {
                tcs.TrySetResult(Convert.ToInt32(completedProcess(process.ExitCode)));
                process.WaitForExit(timeoutInMs);
                //process.Dispose();
            };         

              process.Start();
              return tcs.Task;
        }

        public ScanResult completedProcess(int ExitCode)
        {

            switch (ExitCode)
            {
                case 0:
                    return ScanResult.NoThreatFound;
                case 2:
                    return ScanResult.ThreatFound;
                case 3:
                    return ScanResult.Timeout;
                case 4:
                    return ScanResult.Error;
                default:
                    return ScanResult.Error;
            }

        }
    }
}
