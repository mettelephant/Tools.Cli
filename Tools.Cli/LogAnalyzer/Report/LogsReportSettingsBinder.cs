using System.CommandLine;
using System.CommandLine.Binding;
using Tools.Cli.Commands;
using Tools.Cli.Logging;
using Tools.Core.Settings;

namespace Tools.Cli.LogAnalyzer.Report;

public class LogsReportSettingsBinder(
    Option<DirectoryInfo> reportsDirectoryOption,
    Option<LogLevel?> logLevelOption,
    Option<DirectoryInfo> logDirectoryOption,
    Option<bool> enableConsoleLoggingOption)
    : BaseSettingsBinder<LogsReportSettings>(logLevelOption, logDirectoryOption, enableConsoleLoggingOption)
{
    protected override LogsReportSettings GetBoundValue(BindingContext bindingContext)
    {
        var settings = BindBaseSettings(bindingContext);

        var parseResult = bindingContext.ParseResult;

        settings = settings with { ReportsDirectory = parseResult.GetValueForOption(reportsDirectoryOption)! };

        return settings;
    }
}