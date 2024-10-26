using System.CommandLine;
using Tools.Cli.Commands;

namespace Tools.Cli.LogAnalyzer.Report;

public class LogReportCommand : Command
{
    public LogReportCommand() : base("generate-report", "Generates an HTML report from multiple logs")
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
        var handler = new LogsReportHandler();
        this.SetHandler(
            handler.Handle,
            new LogsReportSettingsBinder(
                reportsDirectoryOption,
                logLevelOption,
                logDirectoryOption,
                enableConsoleLoggingOption));
    }
}