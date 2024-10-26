using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Parsing;
using System.Globalization;
using Tools.Cli.Logging;
using Tools.Core.Settings;

namespace Tools.Cli.Commands;

public abstract class BaseSettingsBinder<TSettings>(
    Option<LogLevel?> logLevelOption,
    Option<DirectoryInfo> logDirectoryOption,
    Option<bool> enableConsoleLoggingOption)
    : BinderBase<TSettings>
    where TSettings : BaseSettings, new()
{
    protected TSettings BindBaseSettings(BindingContext context)
    {
        TSettings settings = new TSettings();
        var parseResult = context.ParseResult;

        settings = settings with
        {
            LogLevel = parseResult.GetValueForOption(logLevelOption),
            LogDirectory = parseResult.GetValueForOption(logDirectoryOption),
            EnableConsoleLogging = parseResult.GetValueForOption(enableConsoleLoggingOption)
        };

        var logLevelSpecified = parseResult.HasOption(logLevelOption);
        var logDirectorySpecified = parseResult.HasOption(logDirectoryOption);

        if (!logLevelSpecified || settings.LogLevel == null || logDirectorySpecified)
        {
            return settings;
        }

        var timestamp = DateTime.Now.ToString("yyyy_MM_dd_HH_mm", CultureInfo.InvariantCulture);
        var logsDirPath = Path.Combine("Logs", timestamp);
        settings = settings with { LogDirectory = new DirectoryInfo(logsDirPath) };

        if (!settings.LogDirectory.Exists)
        {
            settings.LogDirectory.Create();
        }

        return settings;
    }
}