using System.CommandLine;
using Tools.Cli.LogAnalyzer.Analyzer;
using Tools.Cli.LogAnalyzer.Report;

namespace Tools.Cli.LogAnalyzer;

public class LogsCommand : Command
{
    public LogsCommand() : base("logs", "Performs operations on logs")
    {
        // Add subcommands to 'logs' command
        AddCommand(new LogAnalyzerCommand());
        AddCommand(new LogReportCommand());
    }
}