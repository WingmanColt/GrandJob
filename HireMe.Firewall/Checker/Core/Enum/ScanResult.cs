namespace HireMe.Firewall.Checker.Core.Enum
{
    using System.ComponentModel;

    public enum ScanResult : int
    {
        [Description("No threat found")]
        NoThreatFound = 0,

        [Description("Threat found")]
        ThreatFound = 1,

        [Description("The file could not be found")]
        FileNotFound = 2,

        [Description("Timeout")]
        Timeout = 3,

        [Description("Error")]
        Error = 4

    }
}