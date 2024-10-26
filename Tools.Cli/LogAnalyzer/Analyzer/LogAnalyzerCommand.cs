using System.CommandLine;
using Tools.Cli.Commands;

namespace Tools.Cli.LogAnalyzer.Analyzer;

public class LogAnalyzerCommand : Command
{
    public LogAnalyzerCommand() : base("analyze-logs", "Analyzes and combines multiple log files into one")
    {
        // Define specific options
        var separatorOption = new Option<string>(
            aliases: ["--separator", "-s"],
            description: "The separator for the log files")
        {
            IsRequired = true
        };

        // Add logging options
        var (logLevelOption, logDirectoryOption, enableConsoleLoggingOption) = this.AddLoggingOptions();

        // Add specific options
        AddOption(separatorOption);

        // Set handler
        var handler = new LogsAnalyzeHandler();
        this.SetHandler(
            handler.Handle,
            new LogAnalyzerSettingsBinder(
                separatorOption,
                logLevelOption,
                logDirectoryOption,
                enableConsoleLoggingOption));
    }
}