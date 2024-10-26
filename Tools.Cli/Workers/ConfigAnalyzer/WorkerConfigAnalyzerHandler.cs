using CodeMerger.Libraries;
using Spectre.Console;
using Tools.Cli.Commands;
using Tools.Core.Settings;

namespace Tools.Cli.Workers.ConfigAnalyzer;

public class WorkerConfigAnalyzerHandler : BaseHandler<WorkerConfigChangeSettings>
{
    public void Handle(WorkerConfigChangeSettings changeSettings)
    {
        ConfigureLogger(changeSettings);
    }

    protected override void Execute(ProgressContext? progressContext, WorkerConfigChangeSettings changeSettings)
    {
        // Call the static method with settings
        WorkerConfigChangeService.DoIt(changeSettings);
    }
}