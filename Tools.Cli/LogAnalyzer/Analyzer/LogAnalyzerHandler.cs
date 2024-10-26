using Spectre.Console;
using Tools.Cli.Commands;
using Tools.Core.Settings;

namespace Tools.Cli.LogAnalyzer.Analyzer;

public class LogsAnalyzeHandler : BaseHandler<LogAnalyzerSettings>
{
    public void Handle(LogAnalyzerSettings settings)
    {
        ConfigureLogger(settings);
    }

    protected override void Execute(ProgressContext? progressContext, LogAnalyzerSettings settings)
    {
        // Call the static method with settings
        CodeMerger.Libraries.LogAnalyzer.DoIt(settings);
    }
}