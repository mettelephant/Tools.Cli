using System.CommandLine;
using Tools.Cli.LogAnalyzer.Analyzer;
using Tools.Cli.LogAnalyzer.Report;
using Tools.Cli.Workers.ChangeDetector;
using Tools.Cli.Workers.ConfigAnalyzer;

namespace Tools.Cli.Workers;

public class WorkersCommand : Command
{
    public WorkersCommand() : base("workers", "Performs operations on EHS workers")
    {
        // Add subcommands to 'logs' command
        AddCommand(new WorkerChangeDetectorCommand());
        AddCommand(new WorkerConfigAnalyzerCommand());
    }
}