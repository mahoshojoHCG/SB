using CommandLine;

namespace ReportCtl
{
    public class CommandLineOption
    {
        [Option('a', "add", HelpText = "Add a new user or modify existing user.")]
        public bool Add { get; set; }

        [Option('d', "remove", HelpText = "Remove a existing user.")]
        public bool Remove { get; set; }

        [Option('r', "report", HelpText = "One shot report.")]
        public bool Report { get; set; }

        [Option("name", HelpText = "Username to add.", Required = true)]
        public string UserName { get; set; }
    }
}