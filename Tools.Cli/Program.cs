using System.CommandLine;
using Tools.Cli.CodeMerge;
using Tools.Cli.LogAnalyzer;
using Tools.Cli.Workers;

var rootCommand = new RootCommand("Your Application Description");

// Add commands
rootCommand.AddCommand(new MergeCommand());
rootCommand.AddCommand(new LogsCommand());
rootCommand.AddCommand(new WorkersCommand());

// Invoke the root command
await rootCommand.InvokeAsync(args);