using System;

namespace AutoReport
{
    public static class StaticConfig
    {
        public static string ConfigFile => OperatingSystem.IsWindows() ? "config.yml" : "/etc/AutoReport/config.yml";
    }
}