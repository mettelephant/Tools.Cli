using System.CommandLine;
using System.CommandLine.Binding;
using Tools.Cli.Commands;
using Tools.Cli.Logging;
using Tools.Core.Settings;

namespace Tools.Cli.LogAnalyzer.Analyzer;

public class LogAnalyzerSettingsBinder(
    Option<string> separatorOption,
    Option<LogLevel?> logLevelOption,
    Option<DirectoryInfo> logDirectoryOption,
    Option<bool> enableConsoleLoggingOption)
    : BaseSettingsBinder<LogAnalyzerSettings>(logLevelOption, logDirectoryOption, enableConsoleLoggingOption)
{
    protected override LogAnalyzerSettings GetBoundValue(BindingContext bindingContext)
    {
        var settings = BindBaseSettings(bindingContext);

        var parseResult = bindingContext.ParseResult;

        var separator = string.IsNullOrWhiteSpace(parseResult.GetValueForOption(separatorOption))
            ? ","
            : parseResult.GetValueForOption(separatorOption);
        settings = settings with
        {
            Separator = separator!
        };

        return settings;
    }
}