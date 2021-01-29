using System;
using System.IO;

namespace AutoReport
{
    public static class StaticConfig
    {
        public static string ConfigFile => OperatingSystem.IsWindows() ? "config.yml" : "/etc/AutoReport/config.yml";

        public static string LogLocation => OperatingSystem.IsWindows()
            ? Path.Combine(Environment.CurrentDirectory, "logs")
            : "/var/log/AutoReport";
    }
}