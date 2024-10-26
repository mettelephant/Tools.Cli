using Spectre.Console;
using Tools.Cli.Commands;
using Tools.Core.Settings;

namespace Tools.Cli.CodeMerge;

public class MergeHandler : BaseHandler<MergeSettings>
{
    public void Handle(MergeSettings settings)
    {
        ConfigureLogger(settings);
    }

    protected override void Execute(ProgressContext? progressContext, MergeSettings settings)
    {
        // Call the static method with settings
        CodeMerger.Libraries.CodeMerger.DoIt(settings);
    }
}