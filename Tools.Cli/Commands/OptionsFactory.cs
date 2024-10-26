using System.CommandLine;
using Tools.Cli.Logging;

namespace Tools.Cli.Commands;

public static class OptionsFactory
{
    public static Option<LogLevel?> CreateLogLevelOption()
    {
        return new Option<LogLevel?>(
            aliases: ["--log-level", "-l"],
            description: "The level of logging (Debug, Summary, Warning, Error)",
            getDefaultValue: () => LogLevel.None)
        {
            Arity = ArgumentArity.ZeroOrOne,
        };
    }

    public static Option<DirectoryInfo> CreateLogDirectoryOption()
    {
        return new Option<DirectoryInfo>(
            aliases: ["--log-directory", "-ld"],
            description: "The directory to store logs")
        {
            Arity = ArgumentArity.ZeroOrOne,
        };
    }

    public static Option<bool> CreateEnableConsoleLoggingOption()
    {
        return new Option<bool>(
            aliases: ["--console", "-cl"],
            description: "Enable console logging and progress reporting",
            getDefaultValue: () => true)
        {
            Arity = ArgumentArity.ZeroOrOne,
        };
    }

    public static Option<DirectoryInfo> CreateRequiredDirectoryOption(string[] aliases, string description)
    {
        var option = new Option<DirectoryInfo>(aliases, description)
        {
            IsRequired = true,
            Arity = ArgumentArity.ExactlyOne
        };
        return option;
    }

    public static Option<string> CreateRequiredStringOption(string[] aliases, string description)
    {
        var option = new Option<string>(aliases, description)
        {
            IsRequired = true,
            Arity = ArgumentArity.ExactlyOne
        };
        return option;
    }

    // Other common options can be added here
}