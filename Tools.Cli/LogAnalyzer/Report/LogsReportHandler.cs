using CodeMerger.Libraries;
using Spectre.Console;
using Tools.Cli.Commands;
using Tools.Core.Settings;

namespace Tools.Cli.LogAnalyzer.Report;

public class LogsReportHandler : BaseHandler<LogsReportSettings>
{
    public void Handle(LogsReportSettings settings)
    {
        ConfigureLogger(settings);
    }

    protected override void Execute(ProgressContext? progressContext, LogsReportSettings settings)
    {
        // Call the static method with settings
        CodeMergerLogReporter.DoIt(settings);
    }
}