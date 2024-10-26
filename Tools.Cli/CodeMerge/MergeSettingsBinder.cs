using System.CommandLine;
using System.CommandLine.Binding;
using Tools.Cli.Commands;
using Tools.Cli.Logging;
using Tools.Core.Settings;

namespace Tools.Cli.CodeMerge;

public class MergeSettingsBinder(
    Option<DirectoryInfo> svnDirectoryOption,
    Option<DirectoryInfo> gitDirectoryOption,
    Option<string> customerCodeOption,
    Option<bool> cleanSvnOption,
    Option<bool> cleanGitOption,
    Option<LogLevel?> logLevelOption,
    Option<DirectoryInfo> logDirectoryOption,
    Option<bool> enableConsoleLoggingOption)
    : BaseSettingsBinder<MergeSettings>(logLevelOption, logDirectoryOption, enableConsoleLoggingOption)
{
    protected override MergeSettings GetBoundValue(BindingContext bindingContext)
    {
        var settings = BindBaseSettings(bindingContext);

        var parseResult = bindingContext.ParseResult;

        settings = settings with
        {
            SvnDirectory = parseResult.GetValueForOption(svnDirectoryOption)!,
            GitDirectory = parseResult.GetValueForOption(gitDirectoryOption)!,
            CustomerCode = parseResult.GetValueForOption(customerCodeOption)!,
            CleanSvn = parseResult.GetValueForOption(cleanSvnOption),
            CleanGit = parseResult.GetValueForOption(cleanGitOption)
        };

        return settings;
    }
}