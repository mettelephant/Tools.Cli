using CodeMerger.Libraries;
using Spectre.Console;
using Tools.Cli.Commands;
using Tools.Core.Settings;

namespace Tools.Cli.Workers.ChangeDetector;

public class WorkerDetectChangeHandler : BaseHandler<WorkerDetectChangeSettings>
{
    public void Handle(WorkerDetectChangeSettings settings)
    {
        ConfigureLogger(settings);
    }

    protected override void Execute(ProgressContext? progressContext, WorkerDetectChangeSettings settings)
    {
        // Call the static method with settings
        WorkerDetectChangeService.DoIt(settings);
    }
}