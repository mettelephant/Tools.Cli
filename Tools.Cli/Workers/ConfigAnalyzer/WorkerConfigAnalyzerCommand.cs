using System.CommandLine;
using Tools.Cli.Commands;

namespace Tools.Cli.Workers.ConfigAnalyzer;

public class WorkerConfigAnalyzerCommand : Command
{
    public WorkerConfigAnalyzerCommand() : base("analyze-config", "Detect changes in the EHS Workers")
    {
        // Define specific options
        var reportsDirectoryOption = new Option<DirectoryInfo>(
            aliases: ["--report-directory", "-rd"],
            description: "The directory for the reports")
        {
            IsRequired = true
        };

        // Add logging options
        var (logLevelOption, logDirectoryOption, enableConsoleLoggingOption) = this.AddLoggingOptions();

        // Add specific options
        AddOption(reportsDirectoryOption);

        // Set handler
        var handler = new WorkerConfigAnalyzerHandler();
        this.SetHandler(
            handler.Handle,
            new WorkerConfigChangeSettingsBinder(
                reportsDirectoryOption,
                logLevelOption,
                logDirectoryOption,
                enableConsoleLoggingOption));
    }
}