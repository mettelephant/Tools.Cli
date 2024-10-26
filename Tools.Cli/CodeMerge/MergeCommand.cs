

using System.CommandLine;
using Tools.Cli.Commands;

namespace Tools.Cli.CodeMerge;

public class MergeCommand : Command
{
    public MergeCommand() : base("merge", "Merges code")
    {
        // Define specific options
        var svnDirectoryOption = new Option<DirectoryInfo>(
            aliases: ["--svn-directory", "-s"],
            description: "The SVN directory")
        {
            IsRequired = true
        };

        var gitDirectoryOption = new Option<DirectoryInfo>(
            aliases: ["--git-directory", "-g"],
            description: "The Git directory")
        {
            IsRequired = true
        };

        var customerCodeOption = new Option<string>(
            aliases: ["--customer-code", "-c"],
            description: "The customer code")
        {
            IsRequired = true
        };

        var cleanSvnOption = new Option<bool>(
            aliases: ["--clean-svn", "-cs"],
            description: "Clean the SVN directory before merging",
            getDefaultValue: () => false);

        var cleanGitOption = new Option<bool>(
            aliases: ["--clean-git", "-cg"],
            description: "Clean the Git directory before merging",
            getDefaultValue: () => false);

        // Add logging options
        var (logLevelOption, logDirectoryOption, enableConsoleLoggingOption) = this.AddLoggingOptions();

        // Add specific options
        AddOption(svnDirectoryOption);
        AddOption(gitDirectoryOption);
        AddOption(customerCodeOption);
        AddOption(cleanSvnOption);
        AddOption(cleanGitOption);

        // Set handler
        var handler = new MergeHandler();
        this.SetHandler(
            handler.Handle,
            new MergeSettingsBinder(
                svnDirectoryOption,
                gitDirectoryOption,
                customerCodeOption,
                cleanSvnOption,
                cleanGitOption,
                logLevelOption,
                logDirectoryOption,
                enableConsoleLoggingOption));
    }
}