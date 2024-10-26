using System.CommandLine;
using Tools.Cli.Commands;

namespace Tools.Cli.Workers.ChangeDetector;

public class WorkerChangeDetectorCommand : Command
{
    public WorkerChangeDetectorCommand() : base("detect-changes", "Detect changes in the EHS Workers")
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
        var handler = new WorkerDetectChangeHandler();
        this.SetHandler(
            handler.Handle,
            new WorkerDetectChangeSettingsBinder(
                reportsDirectoryOption,
                logLevelOption,
                logDirectoryOption,
                enableConsoleLoggingOption));
    }
}