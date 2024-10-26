using System.CommandLine;
using Tools.Cli.Logging;

namespace Tools.Cli.Commands;

public static class CommandExtensions
{
    public static (Option<LogLevel?> logLevelOption, Option<DirectoryInfo> logDirectoryOption, Option<bool> enableConsoleLoggingOption) AddLoggingOptions(this Command command)
    {
        var logLevelOption = OptionsFactory.CreateLogLevelOption();
        var logDirectoryOption = OptionsFactory.CreateLogDirectoryOption();
        var enableConsoleLoggingOption = OptionsFactory.CreateEnableConsoleLoggingOption();

        command.AddGlobalOption(logLevelOption);
        command.AddGlobalOption(logDirectoryOption);
        command.AddGlobalOption(enableConsoleLoggingOption);

        return (logLevelOption, logDirectoryOption, enableConsoleLoggingOption);
    }
}