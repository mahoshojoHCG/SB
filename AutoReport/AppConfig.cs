using System.Collections.Generic;

namespace AutoReport
{
    public class AppConfig
    {
        public int ReportAt { get; set; }
        public Dictionary<string, ReportInformation> Reports { get; set; } = new();
    }
}