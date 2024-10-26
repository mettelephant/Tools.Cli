﻿using System.CommandLine;
using System.CommandLine.Binding;
using Tools.Cli.Commands;
using Tools.Cli.Logging;
using Tools.Core.Settings;

namespace Tools.Cli.Workers.ChangeDetector;

public class WorkerDetectChangeSettingsBinder(
    Option<DirectoryInfo> reportsDirectoryOption,
    Option<LogLevel?> logLevelOption,
    Option<DirectoryInfo> logDirectoryOption,
    Option<bool> enableConsoleLoggingOption)
    : BaseSettingsBinder<WorkerDetectChangeSettings>(logLevelOption, logDirectoryOption, enableConsoleLoggingOption)
{
    protected override WorkerDetectChangeSettings GetBoundValue(BindingContext bindingContext)
    {
        var settings = BindBaseSettings(bindingContext);

        var parseResult = bindingContext.ParseResult;

        settings = settings with { ReportsDirectory = parseResult.GetValueForOption(reportsDirectoryOption)! };

        return settings;
    }
}