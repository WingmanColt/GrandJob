namespace HireMe.Firewall.Checker.Core.Interface
{
    using HireMe.Firewall.Checker.Core.Enum;
    using System.Threading.Tasks;

    public interface IScanner
    {
        /// The maximum time in milliseconds to take for this scan
        /// <returns>The scan result</returns>
        ScanResult Scan(string file, int timeoutInMs = 30000);

        Task<int> ScanAsync(string file, int timeoutInMs = 30000);

        ScanResult completedProcess(int ExitCode);
    }
}
