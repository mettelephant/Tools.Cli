using System.CommandLine;
using Tools.Cli.CodeMerge;
using Tools.Cli.Logs;

var rootCommand = new RootCommand("Your Application Description");

// Add commands
rootCommand.AddCommand(new MergeCommand());
rootCommand.AddCommand(new LogsCommand());
//rootCommand.AddCommand(new WorkerCommand());

// Invoke the root command
await rootCommand.InvokeAsync(args);